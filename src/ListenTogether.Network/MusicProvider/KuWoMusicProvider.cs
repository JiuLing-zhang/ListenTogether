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
    public KuWoMusicProvider()
    {
        var handler = new HttpClientHandler();
        handler.CookieContainer = _cookieContainer;
        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        _httpClient = new HttpClient(handler);
    }

    private async Task InitCookie()
    {
        await _httpClient.GetStringAsync("http://www.kuwo.cn").ConfigureAwait(false);
    }

    public Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> SearchAsync(string keyword)
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

        if (_cookieContainer.Count == 0)
        {
            await InitCookie();
        }

        var csrf = _cookieContainer.GetCookies(new Uri("http://www.kuwo.cn"))["kw_token"]?.Value;
        if (csrf.IsEmpty())
        {
            Logger.Error("酷我音乐搜索参数构造失败。", new ArgumentNullException("csrf"));
            return (false, "参数构造失败", null);
        }
        request.Headers.Add("csrf", csrf);

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
            return (false, "解析数据失败", null);
        }
        if (httpResult == null)
        {
            return (false, "请求服务器失败", null);
        }
        if (httpResult.code != 200)
        {
            return (false, httpResult.message, null);
        }

        var musics = new List<MusicSearchResult>();
        if (httpResult.data.list.Length == 0)
        {
            return (true, "", new List<MusicSearchResult>());
        }

        foreach (var httpMusic in httpResult.data.list)
        {
            try
            {
                var music = new MusicSearchResult()
                {
                    Id = MD5Utils.GetStringValueToLower($"{Platform}-{httpMusic.rid}"),
                    Platform = Platform,
                    PlatformInnerId = httpMusic.rid.ToString(),
                    Name = httpMusic.name.Replace("&nbsp;", " "),
                    Alias = "",
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
        return (true, "", musics);
    }

    private FeeEnum GetFeeFlag(string listenFragment)
    {
        if (listenFragment == "1")
        {
            return FeeEnum.Demo;
        }

        return FeeEnum.Free;
    }

    public async Task<Music?> GetDetailAsync(MusicSearchResult sourceMusic, MusicFormatTypeEnum musicFormatType)
    {
        string musicId = sourceMusic.PlatformInnerId;

        //获取歌曲详情
        var url = $"{UrlBase.KuWo.GetMusicDetail}?musicId={musicId}";
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
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回异常，ID:{musicId}"));
            return null;
        }
        if (httpResult.status != 200)
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回状态异常：{httpResult.message ?? ""}，ID:{musicId}"));
            return null;
        }

        return new Music()
        {
            Id = sourceMusic.Id,
            Platform = sourceMusic.Platform,
            PlatformName = sourceMusic.Platform.GetDescription(),
            PlatformInnerId = sourceMusic.PlatformInnerId,
            Name = sourceMusic.Name,
            Alias = sourceMusic.Alias,
            Artist = sourceMusic.Artist,
            Album = sourceMusic.Album,
            ImageUrl = httpResult.data.songinfo.pic,
            ExtendData = ""
        };
    }
    public async Task<string?> GetPlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType)
    {
        string musicId = music.PlatformInnerId;
        //换播放地址
        string url = $"{UrlBase.KuWo.GetMusicUrl}?format=mp3&rid={musicId}&response=url&type=convert_url";
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
                Logger.Error("更新酷我播放地址失败。", new Exception($"服务器返回空，ID:{musicId}"));
                return null;
            }
            if (!JiuLing.CommonLibs.Text.RegexUtils.IsMatch(playUrl, "http\\S*\\.mp3"))
            {
                Logger.Error("更新酷我播放地址失败。", new Exception($"服务器返回：{playUrl}，ID:{musicId}"));
                return null;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("更新酷我播放地址失败。", ex);
            return null;
        }
        return playUrl;
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

    public Task<string> GetShareUrlAsync(Music music)
    {
        return Task.FromResult($"{UrlBase.KuWo.GetMusicPlayPage}/{music.PlatformInnerId}");
    }

    public async Task<string?> GetLyricAsync(Music music)
    {
        string musicId = music.PlatformInnerId;

        //获取歌曲详情
        var url = $"{UrlBase.KuWo.GetMusicDetail}?musicId={musicId}";
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
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回异常，ID:{musicId}"));
            return null;
        }
        if (httpResult.status != 200)
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回状态异常：{httpResult.message ?? ""}，ID:{musicId}"));
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
}