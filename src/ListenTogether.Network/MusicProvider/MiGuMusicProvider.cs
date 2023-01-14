using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
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

        Task.Run(async () =>
        {
            await InitCommonArgs();
        });
    }

    private async Task InitCommonArgs()
    {
        var html = await _httpClient.GetStringAsync("https://music.migu.cn/v3").ConfigureAwait(false);

        string pattern = @"SOURCE_ID\s*:\s*'(?<SOURCE_ID>\d+)'\s*,";
        var (_, sourceId) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);

        pattern = @"CHANNEL_ID\s*:\s*'(?<CHANNEL_ID>\S+)'\s*,";
        var (_, channelId) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);

        pattern = @"APP_VERSION\s*:\s*'(?<APP_VERSION>\S+)'\s*,";
        var (_, appVersion) = JiuLing.CommonLibs.Text.RegexUtils.GetOneGroupInFirstMatch(html, pattern);

        MiGuUtils.SetCommonArgs(sourceId, channelId, appVersion);
    }


    public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> SearchAsync(string keyword)
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
            return (false, "数据解析失败", null);
        }

        var musics = new List<MusicSearchResult>();
        foreach (var htmlMusic in htmlMusics)
        {
            musics.Add(new MusicSearchResult()
            {
                Id = MD5Utils.GetStringValueToLower($"{Platform}-{htmlMusic.id}"),
                Platform = Platform,
                PlatformInnerId = htmlMusic.id,
                Name = htmlMusic.title,
                Alias = "",
                Artist = htmlMusic.singer,
                Album = htmlMusic.album,
                ImageUrl = htmlMusic.imgUrl,
                Fee = FeeEnum.Free,
            });
        }
        return (true, "", musics);
    }

    public async Task<Music?> GetDetailAsync(MusicSearchResult sourceMusic, MusicFormatTypeEnum musicFormatType)
    {
        string url = $"{UrlBase.MiGu.GetMusicDetailUrl}?copyrightId={sourceMusic.PlatformInnerId}&resourceType=2";
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

        string imageUrl = sourceMusic.ImageUrl;
        if (result.resource[0].albumImgs != null && result.resource[0].albumImgs.Count > 0)
        {
            string tmpImageUrl = result.resource[0].albumImgs.FirstOrDefault(x => x.imgSizeType == "02")?.img ?? "";
            if (tmpImageUrl.IsNotEmpty())
            {
                imageUrl = tmpImageUrl;
            }
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
            Album = result.resource[0].album,
            ImageUrl = imageUrl,
            ExtendData = ""
        };
    }

    public async Task<string?> GetPlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType)
    {
        string url = $"{UrlBase.MiGu.GetMusicDetailUrl}?copyrightId={music.PlatformInnerId}&resourceType=2";
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

        string playUrlPath = MiGuUtils.GetPlayUrlPath(result.resource[0].newRateFormats, musicFormatType);
        if (playUrlPath.IsEmpty())
        {
            return null;
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

    public Task<string> GetShareUrlAsync(Music music)
    {
        return Task.FromResult($"{UrlBase.MiGu.GetMusicPlayPage}/{music.PlatformInnerId}");
    }

    public async Task<string?> GetLyricAsync(Music music)
    {
        string url = $"{UrlBase.MiGu.GetMusicDetailUrl}?copyrightId={music.PlatformInnerId}&resourceType=2";
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
        var html = await _httpClient.GetStringAsync(url);
        return MiGuUtils.GetTags(html);
    }

    public async Task<List<SongMenu>> GetSongMenusFromTagAsync(string id)
    {
        string url = $"{UrlBase.MiGu.GetMusicTagPlayUrl}?tagId={id}";
        var html = await _httpClient.GetStringAsync(url);
        return MiGuUtils.GetSongMenusFromTag(html);
    }
    public Task<List<SongMenu>> GetSongMenusFromTop()
    {
        return Task.FromResult(MiGuUtils.GetSongMenusFromTop());
    }
}