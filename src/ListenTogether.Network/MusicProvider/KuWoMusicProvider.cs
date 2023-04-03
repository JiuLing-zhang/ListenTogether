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
    private readonly string _reqId = JiuLing.CommonLibs.GuidUtils.GetFormatD();
    public string _csrf
    {
        get
        {
            var csrf = _cookieContainer.GetCookies(new Uri("http://www.kuwo.cn"))["kw_token"]?.Value ?? "";
            if (csrf.IsNotEmpty())
            {
                return csrf;
            }
            var task = Task.Run(InitCookie);
            task.Wait();
            csrf = _cookieContainer.GetCookies(new Uri("http://www.kuwo.cn"))["kw_token"]?.Value ?? "";
            return csrf;
        }
    }
    public KuWoMusicProvider()
    {
        var handler = new HttpClientHandler();
        handler.CookieContainer = _cookieContainer;
        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        _httpClient = new HttpClient(handler);
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }
    private async Task InitCookie()
    {
        await _httpClient.GetStringAsync("http://www.kuwo.cn");
    }

    public Task<List<string>> GetSearchSuggestAsync(string keyword)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MusicResultShow>> SearchAsync(string keyword)
    {
        var musics = new List<MusicResultShow>();

        try
        {
            string url = $"{UrlBase.KuWo.Search}?key={keyword}&pn=1&rn=20&httpsStatus=1&reqId={_reqId}";
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
                        ImageUrl = httpMusic.pic,
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

    public async Task<List<MusicResultShow>> GetTopMusicsAsync(string topId)
    {
        var musics = new List<MusicResultShow>();
        try
        {
            string url = $"{UrlBase.KuWo.GetTopMusicsUrl}?bangId={topId}&pn=1&rn=20&httpsStatus=1&reqId={_reqId}";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
            request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
            request.Headers.Add("Referer", "http://www.kuwo.cn/rankList");
            request.Headers.Add("Host", "www.kuwo.cn");
            request.Headers.Add("csrf", _csrf);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var bangMusics = KuWoUtils.GetSongMenuMusics(json);

            foreach (var bangMusic in bangMusics)
            {
                try
                {
                    musics.Add(new MusicResultShow()
                    {
                        Id = MD5Utils.GetStringValueToLower($"{Platform}-{bangMusic.rid}"),
                        Platform = Platform,
                        IdOnPlatform = bangMusic.rid.ToString(),
                        Name = bangMusic.name.Replace("&nbsp;", " "),
                        Artist = bangMusic.artist.Replace("&nbsp;", " "),
                        Album = bangMusic.album.Replace("&nbsp;", " "),
                        Fee = GetFeeFlag(bangMusic.payInfo.listen_fragment),
                        Duration = TimeSpan.FromSeconds(bangMusic.duration),
                        ImageUrl = bangMusic.pic
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("酷我榜单歌曲添加失败。", ex);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("酷我榜单歌曲获取失败。", ex);
        }
        return musics;
    }

    public async Task<List<string>> GetHotWordAsync()
    {
        var reslt = new List<string>();
        try
        {
            string url = $"{UrlBase.KuWo.HotWord}?key=&httpsStatus=1&reqId={_reqId}";
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
            reslt = KuWoUtils.GetHotWord(json);
        }
        catch (Exception ex)
        {
            Logger.Error("酷我热搜词获取失败。", ex);
        }
        return reslt;
    }

    public Task<string> GetShareUrlAsync(string id, string extendDataJson = "")
    {
        return Task.FromResult($"{UrlBase.KuWo.GetMusicPlayPage}/{id}");
    }

    public async Task<string> GetLyricAsync(string id, string extendDataJson = "")
    {
        //获取歌曲详情
        var url = $"{UrlBase.KuWo.GetMusicDetail}?musicId={id}&httpsStatus=1&reqId={_reqId}";
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
            return "";
        }
        if (httpResult == null)
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回异常，ID:{id}"));
            return "";
        }
        if (httpResult.status != 200)
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回状态异常：{httpResult.message ?? ""}，ID:{id}"));
            return "";
        }

        //处理歌词
        var sbLyrics = new StringBuilder();
        if (httpResult.data.lrclist != null && httpResult.data.lrclist.Count > 0)
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

        string url = $"{UrlBase.KuWo.GetAllTypesUrl}?httpsStatus=1&reqId={_reqId}";
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

    public async Task<List<SongMenu>> GetSongMenusFromTagAsync(string id, int page)
    {
        var songMenus = new List<SongMenu>();
        try
        {
            string url = $"{UrlBase.KuWo.GetTagSongMenuUrl}?pn={page}&rn=20&id={id}&httpsStatus=1&reqId={_reqId}";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
            request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
            request.Headers.Add("Referer", "http://www.kuwo.cn/playlists");
            request.Headers.Add("Host", "www.kuwo.cn");
            request.Headers.Add("csrf", _csrf);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var tagSongMenus = KuWoUtils.GetTagSongMenus(json);

            foreach (var tagSongMenu in tagSongMenus)
            {
                try
                {
                    songMenus.Add(new SongMenu()
                    {
                        Id = tagSongMenu.id,
                        ImageUrl = tagSongMenu.img,
                        Name = tagSongMenu.name,
                        LinkUrl = ""
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("酷我标签歌单添加失败。", ex);
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("酷我标签歌单获取失败。", ex);
        }
        return songMenus;
    }

    public async Task<List<MusicResultShow>> GetTagMusicsAsync(string tagId)
    {
        var musics = new List<MusicResultShow>();
        try
        {
            string url = $"{UrlBase.KuWo.GetTagMusicsUrl}?pid={tagId}&pn=1&rn=20&httpsStatus=1&reqId={_reqId}";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
            request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
            request.Headers.Add("Referer", "http://www.kuwo.cn/playlists");
            request.Headers.Add("Host", "www.kuwo.cn");
            request.Headers.Add("csrf", _csrf);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var bangMusics = KuWoUtils.GetSongMenuMusics(json);

            foreach (var bangMusic in bangMusics)
            {
                try
                {
                    musics.Add(new MusicResultShow()
                    {
                        Id = MD5Utils.GetStringValueToLower($"{Platform}-{bangMusic.rid}"),
                        Platform = Platform,
                        IdOnPlatform = bangMusic.rid.ToString(),
                        Name = bangMusic.name.Replace("&nbsp;", " "),
                        Artist = bangMusic.artist.Replace("&nbsp;", " "),
                        Album = bangMusic.album.Replace("&nbsp;", " "),
                        Fee = GetFeeFlag(bangMusic.payInfo.listen_fragment),
                        Duration = TimeSpan.FromSeconds(bangMusic.duration),
                        ImageUrl = bangMusic.pic
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("酷我榜单歌曲添加失败。", ex);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("酷我榜单歌曲获取失败。", ex);
        }
        return musics;
    }

    public async Task<string> GetPlayUrlAsync(string id, string extendDataJson = "")
    {
        //换播放地址
        string url = $"{UrlBase.KuWo.GetMusicUrl}?format=aac|mp3&rid={id}&response=url&type=convert_url";
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
            if (!JiuLing.CommonLibs.Text.RegexUtils.IsMatch(playUrl, @"http\S*?\.aac|mp3"))
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