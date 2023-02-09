using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.Models.KuWo;
using ListenTogether.Network.Utils;
using System.Net;
using System.Text;

namespace ListenTogether.Network.MusicProvider;
internal class KuWoMusicProvider : IMusicProvider
{
    private readonly HttpClient _httpClient;
    private readonly CookieContainer _cookieContainer = new CookieContainer();
    private const PlatformEnum Platform = PlatformEnum.KuWo;
    private string _csrf => _cookieContainer.GetCookies(new Uri("http://www.kuwo.cn"))["kw_token"]?.Value ?? "";
    public KuWoMusicProvider()
    {
        var handler = new HttpClientHandler();
        handler.CookieContainer = _cookieContainer;
        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        _httpClient = new HttpClient(handler);
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
        Task.Run(InitCookie);
    }

    private async Task InitCookie()
    {
        await _httpClient.GetStringAsync("http://www.kuwo.cn").ConfigureAwait(false);
    }

    public Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MusicResultShow>> SearchAsync(string keyword)
    {
        var musics = new List<MusicResultShow>();

        try
        {
            string url = $"{UrlBase.KuWo.Search}?key={keyword}&pn=1&rn=20";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
            request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
            request.Headers.Add("Referer", "http://www.kuwo.cn/");
            request.Headers.Add("Host", "kuwo.cn");
            request.Headers.Add("csrf", _csrf);

            HttpResultBase<HttpMusicSearchResult>? httpResult;
            try
            {
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                httpResult = json.ToObject<HttpResultBase<HttpMusicSearchResult>>();
            }
            catch (Exception ex)
            {
                Logger.Error("解析酷我音乐搜索结果失败。", ex);
                return musics;
            }
            if (httpResult == null || httpResult.code != 200)
            {
                return musics;
            }

            foreach (var httpMusic in httpResult.data.list)
            {
                try
                {
                    var music = new MusicResultShow()
                    {
                        Id = MD5Utils.GetStringValueToLower($"{Platform}-{httpMusic.rid}"),
                        Platform = Platform,
                        IdOnPlatform = httpMusic.rid.ToString(),
                        Name = httpMusic.name.Replace("&nbsp;", " "),
                        Artist = httpMusic.artist.Replace("&nbsp;", " "),
                        Album = httpMusic.album.Replace("&nbsp;", " "),
                        Fee = GetFeeFlag(httpMusic.payInfo.listen_fragment),
                        Duration = TimeSpan.FromSeconds(httpMusic.duration)
                    };
                    musics.Add(music);
                }
                catch (Exception ex)
                {
                    Logger.Error("构建酷狗搜索结果失败。", ex);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("酷我搜索失败。", ex);
        }

        return musics;
    }

    private FeeEnum GetFeeFlag(string listenFragment)
    {
        if (listenFragment == "1")
        {
            return FeeEnum.Demo;
        }

        return FeeEnum.Free;
    }

    public Task<List<SongMenu>> GetSongMenusFromTop()
    {
        return Task.FromResult(KuWoUtils.GetSongMenusFromTop());
    }

    public Task<List<MusicResultShow>> GetTopMusicsAsync(string topId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<string>?> GetHotWordAsync()
    {
        string url = UrlBase.KuWo.HotWord;
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url)
        };
        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        request.Headers.Add("Accept-Encoding", "gzip, deflate");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentIphone);
        request.Headers.Add("Host", "m.kuwo.cn");

        try
        {
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return KuWoUtils.GetHotWordFromHtml(html);
        }
        catch (Exception ex)
        {
            Logger.Error("酷我歌曲播放地址获取失败。", ex);
            return null;
        }
    }

    public Task<string> GetShareUrlAsync(string id, string extendDataJson = "")
    {
        return Task.FromResult($"{UrlBase.KuWo.GetMusicPlayPage}/{id}");
    }

    public async Task<string> GetLyricAsync(string id, string extendDataJson = "")
    {
        //获取歌曲详情
        var url = $"{UrlBase.KuWo.GetMusicDetail}?musicId={id}";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        HttpResultBase<MusicDetailHttpResult>? httpResult;
        try
        {
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            httpResult = json.ToObject<HttpResultBase<MusicDetailHttpResult>>();
        }
        catch (Exception ex)
        {
            Logger.Error("酷我歌曲详情获取失败。", ex);
            return null;
        }
        if (httpResult == null)
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回异常，ID:{id}"));
            return null;
        }
        if (httpResult.status != 200)
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回状态异常：{httpResult.message ?? ""}，ID:{id}"));
            return null;
        }

        //处理歌词
        var sbLyrics = new StringBuilder();
        if (httpResult.data.lrclist != null && httpResult.data.lrclist.Length > 0)
        {
            foreach (var lyricInfo in httpResult.data.lrclist)
            {
                var ts = TimeSpan.FromSeconds(Convert.ToDouble(lyricInfo.time));
                string minutes = ts.Minutes.ToString();
                string seconds = ts.Seconds.ToString();
                string milliseconds = ts.Milliseconds.ToString();
                string time = $"[{minutes.PadLeft(2, '0')}:{seconds.PadLeft(2, '0')}.{milliseconds.PadLeft(3, '0')}]";
                sbLyrics.AppendLine($"{time}{lyricInfo.lineLyric}");
            }
        }

        return sbLyrics.ToString();
    }

    public async Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync()
    {
        string html = await _httpClient.GetStringAsync("http://www.kuwo.cn").ConfigureAwait(false);
        var hotTags = KuWoUtils.GetHotTags(html);

        string url = $"{UrlBase.KuWo.GetAllTypesUrl}";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Accept", "application/json, text/plain, */*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
        request.Headers.Add("Referer", "http://www.kuwo.cn/");
        request.Headers.Add("Host", "www.kuwo.cn");
        request.Headers.Add("csrf", _csrf);
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var allTypes = KuWoUtils.GetAllTypes(json);
        return (hotTags, allTypes);
    }

    public Task<List<SongMenu>> GetSongMenusFromTagAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<MusicResultShow>> GetTagMusicsAsync(string tagId)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetPlayUrlAsync(string id, string extendDataJson = "")
    {
        //换播放地址
        string url = $"{UrlBase.KuWo.GetMusicUrl}?format=mp3&rid={id}&response=url&type=convert_url";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        string playUrl;
        try
        {
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            playUrl = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (playUrl.IsEmpty())
            {
                Logger.Error("更新酷我播放地址失败。", new Exception($"服务器返回空，ID:{id}"));
                return "";
            }
            if (!JiuLing.CommonLibs.Text.RegexUtils.IsMatch(playUrl, "http\\S*\\.mp3"))
            {
                Logger.Error("更新酷我播放地址失败。", new Exception($"服务器返回：{playUrl}，ID:{id}"));
                return "";
            }
        }
        catch (Exception ex)
        {
            Logger.Error("更新酷我播放地址失败。", ex);
            return "";
        }
        return playUrl;
    }

    public Task<string> GetImageUrlAsync(string id, string extendDataJson = "")
    {
        throw new NotImplementedException();
    }
}