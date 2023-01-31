using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.Models.MiGu;
using ListenTogether.Network.Utils;
using System.Net;

namespace ListenTogether.Network.MusicProvider;
public class MiGuMusicProvider : IMusicProvider
{
    private readonly HttpClient _httpClient;
    private const PlatformEnum Platform = PlatformEnum.MiGu;

    public MiGuMusicProvider()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        _httpClient = new HttpClient(handler);
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
        Task.Run(async () =>
        {
            await InitCommonArgs();
        });
    }

    private async Task InitCommonArgs()
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(UrlBase.MiGu.Index),
            Method = HttpMethod.Get
        };
        foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        string pattern = @"SOURCE_ID\s*:\s*'(?<SOURCE_ID>\d+)'\s*,";
        var (_, sourceId) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);

        pattern = @"CHANNEL_ID\s*:\s*'(?<CHANNEL_ID>\S+)'\s*,";
        var (_, channelId) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);

        pattern = @"APP_VERSION\s*:\s*'(?<APP_VERSION>\S+)'\s*,";
        var (_, appVersion) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);

        MiGuUtils.SetCommonArgs(sourceId, channelId, appVersion);
    }


    public async Task<List<MusicResultShow>> SearchAsync(string keyword)
    {
        var musics = new List<MusicResultShow>();
        try
        {
            string args = MiGuUtils.GetSearchArgs(keyword);
            string url = $"{UrlBase.MiGu.Search}?{args}";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            request.Headers.Add("Cookie", $"migu_cookie_id={MiGuUtils.CookieId}");
            request.Headers.Add("Host", "music.migu.cn");
            request.Headers.Add("Referer", UrlBase.MiGu.Index);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!MiGuUtils.TryScanSearchResult(html, out var htmlMusics))
            {
                return musics;
            }

            foreach (var htmlMusic in htmlMusics)
            {
                musics.Add(new MusicResultShow()
                {
                    Id = MD5Utils.GetStringValueToLower($"{Platform}-{htmlMusic.id}"),
                    Platform = Platform,
                    IdOnPlatform = htmlMusic.id,
                    Name = htmlMusic.title,
                    Artist = htmlMusic.singer,
                    Album = htmlMusic.album,
                    ImageUrl = htmlMusic.imgUrl,
                    Fee = FeeEnum.Free,
                });
            }
        }
        catch (Exception ex)
        {
            Logger.Error("咪咕搜索失败。", ex);
        }

        return musics;
    }

    public async Task<string> GetPlayUrlAsync(string id, string extendDataJson = "")
    {
        string url = $"{UrlBase.MiGu.GetMusicDetailUrl}?copyrightId={id}&resourceType=2";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Accept", "application/json, text/plain, */*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentIphone);
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var result = json.ToObject<HttpMusicDetailResult>();
        if (result == null || result.resource == null || result.resource.Length == 0)
        {
            return "";
        }

        if (result.resource[0].newRateFormats == null || result.resource[0].newRateFormats.Count == 0)
        {
            return "";
        }

        string playUrlPath = MiGuUtils.GetPlayUrlPath(result.resource[0].newRateFormats, MusicProviderSetting.MusicFormatType);
        if (playUrlPath.IsEmpty())
        {
            return "";
        }
        return $"{UrlBase.MiGu.PlayUrlDomain}{playUrlPath}";
    }

    public Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>?> GetHotWordAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetShareUrlAsync(string id, string extendDataJson = "")
    {
        return Task.FromResult($"{UrlBase.MiGu.GetMusicPlayPage}/{id}");
    }

    public async Task<string> GetLyricAsync(string id, string extendDataJson = "")
    {
        string url = $"{UrlBase.MiGu.GetMusicDetailUrl}?copyrightId={id}&resourceType=2";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Accept", "application/json, text/plain, */*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentIphone);
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var result = json.ToObject<HttpMusicDetailResult>();
        if (result == null || result.resource == null || result.resource.Length == 0)
        {
            return null;
        }

        if (result.resource[0].newRateFormats == null || result.resource[0].newRateFormats.Count == 0)
        {
            return null;
        }

        string lyricUrl = result.resource[0].lrcUrl;
        if (lyricUrl.IsEmpty())
        {
            return null;
        }

        return await _httpClient.GetStringAsync(lyricUrl);
    }

    public async Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync()
    {
        string url = $"{UrlBase.MiGu.GetTagsUrl}";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return MiGuUtils.GetTags(html);
    }

    public async Task<List<SongMenu>> GetSongMenusFromTagAsync(string id)
    {
        string url = $"{UrlBase.MiGu.GetMusicTagPlayUrl}?tagId={id}";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return MiGuUtils.GetSongMenusFromTag(html);
    }
    public Task<List<SongMenu>> GetSongMenusFromTop()
    {
        return Task.FromResult(MiGuUtils.GetSongMenusFromTop());
    }

    public async Task<List<MusicResultShow>> GetTopMusicsAsync(string topId)
    {
        var musics = new List<MusicResultShow>();
        string url = $"{UrlBase.MiGu.GetTopMusicsUrl}{topId}";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        //HttpMusicTopSongItemResult
        var songs = MiGuUtils.GetTopMusics(html);
        foreach (var song in songs)
        {
            var artist = "";
            if (song.singers != null)
            {
                artist = string.Join("、", song.singers.Select(x => x.name));
            }

            //时长 时:分:秒
            TimeSpan duration = TimeSpan.Zero;
            var durationArray = song.duration.Split(":");
            if (durationArray.Length == 3)
            {
                int HH = Convert.ToInt32(durationArray[0]);
                int mm = Convert.ToInt32(durationArray[1]);
                int ss = Convert.ToInt32(durationArray[2]);

                duration = TimeSpan.FromHours(HH);
                duration = duration.Add(TimeSpan.FromMinutes(mm));
                duration = duration.Add(TimeSpan.FromSeconds(ss));
            }

            musics.Add(new MusicResultShow()
            {
                Id = MD5Utils.GetStringValueToLower($"{Platform}-{song.copyrightId}"),
                Platform = Platform,
                IdOnPlatform = song.copyrightId,
                Name = song.name,
                Artist = artist,
                Album = song.album?.albumName ?? "",
                ImageUrl = song.ImageUrl,
                Duration = duration,
                Fee = FeeEnum.Free,
            });
        }
        return musics;
    }

    public async Task<List<MusicResultShow>> GetTagMusicsAsync(string tagId)
    {
        var musics = new List<MusicResultShow>();
        string url = $"{UrlBase.MiGu.GetTagMusicsUrl}{tagId}";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        foreach (var header in JiuLing.CommonLibs.Net.BrowserDefaultHeader.EdgeHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var songs = MiGuUtils.GetTagMusics(html);
        foreach (var song in songs)
        {
            var songId = song.linkUrl;
            if (songId.IsEmpty() || songId.IndexOf("/") < 0)
            {
                continue;
            }
            songId = songId.Substring(songId.LastIndexOf("/") + 1);

            musics.Add(new MusicResultShow()
            {
                Id = MD5Utils.GetStringValueToLower($"{Platform}-{songId}"),
                Platform = Platform,
                IdOnPlatform = songId,
                Name = song.title,
                Artist = song.singer,
                Album = song.album,
                ImageUrl = song.ImageUrl,
                Fee = FeeEnum.Free,
            });
        }
        return musics;
    }

    public Task<string> GetImageUrlAsync(string id, string extendDataJson = "")
    {
        throw new NotImplementedException();
    }
}