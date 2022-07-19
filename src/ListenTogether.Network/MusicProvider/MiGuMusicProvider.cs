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
    }
    public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> Search(string keyword)
    {
        string args = MiGuUtils.GetSearchData(keyword);
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
        request.Headers.Add("Host", "www.migu.cn");
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
                Id = MD5Utils.GetStringValueToLower($"{Platform}-{htmlMusic.Id}"),
                Platform = Platform,
                PlatformInnerId = htmlMusic.Id,
                Name = htmlMusic.Name,
                Alias = "",
                Artist = htmlMusic.Artist,
                ImageUrl = htmlMusic.ImageUrl,
                Fee = FeeEnum.Free,
                PlatformData = new SearchResultExtended()
                {
                    MusicPageUrl = htmlMusic.MusicPageUrl
                }
            });
        }
        return (true, "", musics);
    }

    public async Task<Music?> GetMusicDetail(MusicSearchResult sourceMusic)
    {
        if (!(sourceMusic.PlatformData is SearchResultExtended platformData))
        {
            throw new ArgumentException("平台数据初始化异常");
        }

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(platformData.MusicPageUrl),
            Method = HttpMethod.Get
        };

        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentIphone);
        request.Headers.Add("Host", "www.migu.cn");
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (response.RequestMessage == null || response.RequestMessage.RequestUri == null)
        {
            return null;
        }

        var argsResult = MiGuUtils.GetMusicRealArgs(response.RequestMessage.RequestUri.ToString());
        if (argsResult.success == false)
        {
            return null;
        }

        string url = $"{UrlBase.MiGu.GetMusicDetailUrl}?copyrightId={argsResult.id}&resourceType=2";
        request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Accept", "application/json, text/plain, */*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentIphone);
        response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var result = json.ToObject<HttpMusicDetailResult>();
        if (result == null || result.resource == null || result.resource.Length == 0)
        {
            return null;
        }

        string lyricUrl = result.resource[0].lrcUrl;
        if (lyricUrl.IsEmpty())
        {
            return null;
        }

        string lyric = await _httpClient.GetStringAsync(lyricUrl);
        string contentId = result.resource[0].contentId;
        if (contentId.IsEmpty())
        {
            return null;
        }

        string args = MiGuUtils.GetPlayUrlData(contentId, argsResult.id);
        url = $"{UrlBase.MiGu.GetMusicPlayUrl}?{args}";

        request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url)
        };
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Encoding", "identity;q=1, *;q=0");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentIphone);
        response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.Found || html.IsNotEmpty())
        {
            return null;
        }

        string? playUrl = response.Headers?.Location?.ToString();
        if (playUrl.IsEmpty())
        {
            return null;
        }

        string imageUrl = sourceMusic.ImageUrl;
        if (result.resource[0].albumImgs != null && result.resource[0].albumImgs.Count > 0)
        {
            string tmpImageUrl = result.resource[0].albumImgs.FirstOrDefault(x => x.imgSizeType == "02")?.img;
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
            PlayUrl = playUrl,
            Lyric = lyric,
            ExtendData = ""
        };
    }

    public Task<Music?> UpdatePlayUrl(Music music)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetSearchSuggest(string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>?> GetHotWord()
    {
        throw new NotImplementedException();
    }
}