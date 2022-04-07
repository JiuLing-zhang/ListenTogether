using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Network.Models.MiGu;
using MusicPlayerOnline.Network.Utils;
using System.Net;
using System.Text.Json;

namespace MusicPlayerOnline.Network.MusicProvider;
public class MiGuMusicProvider : IMusicProvider
{
    private readonly HttpClient _httpClient;
    private const PlatformEnum Platform = PlatformEnum.MiGu;

    public MiGuMusicProvider()
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        _httpClient = new HttpClient(clientHandler);


        _httpClient.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Mobile Safari/537.36 Edg/92.0.902.78");

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
                Id = MD5Utils.GetStringValueToLower($"{Platform}-{htmlMusic.Name}-{htmlMusic.Artist}"),
                Platform = Platform,
                Name = htmlMusic.Name,
                Alias = "",
                Artist = htmlMusic.Artist,
                ImageUrl = htmlMusic.ImageUrl,
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
        request.Headers.Add("Host", "www.migu.cn");
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

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
        string json = await _httpClient.GetStringAsync(url).ConfigureAwait(false);

        var result = JsonSerializer.Deserialize<HttpMusicDetailResult>(json);
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
        response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

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
            PlatformInnerId = sourceMusic.PlatformInnerId,
            Name = sourceMusic.Name,
            Alias = sourceMusic.Alias,
            Artist = sourceMusic.Artist,
            Album = result.resource[0].album,
            Duration = sourceMusic.Duration,
            ImageUrl = imageUrl,
            PlayUrl = playUrl,
            Lyric = lyric
        };
    }

    public Task<Music?> UpdatePlayUrl(Music music)
    {
        throw new NotImplementedException();
    }
}