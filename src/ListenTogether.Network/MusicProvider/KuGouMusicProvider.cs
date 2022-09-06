using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.Models.KuGou;
using ListenTogether.Network.Utils;
using System.Net;

namespace ListenTogether.Network.MusicProvider;
public class KuGouMusicProvider : IMusicProvider
{
    private readonly HttpClient _httpClient;
    private const PlatformEnum Platform = PlatformEnum.KuGou;

    public KuGouMusicProvider()
    {
        var handler = new HttpClientHandler();
        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        _httpClient = new HttpClient(handler);
    }

    public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> SearchAsync(string keyword)
    {
        string args = KuGouUtils.GetSearchData(keyword);
        string url = $"{UrlBase.KuGou.Search}?{args}";

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url)
        };
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        json = KuGouUtils.RemoveHttpResultHead(json);
        if (json.IsEmpty())
        {
            return (false, "服务器响应异常", null);
        }

        HttpResultBase<HttpMusicSearchResult>? httpResult;
        try
        {
            httpResult = json.ToObject<HttpResultBase<HttpMusicSearchResult>>();
        }
        catch (Exception ex)
        {
            Logger.Error("解析酷狗搜索结果失败。", ex);
            return (false, "解析数据失败", null);
        }
        if (httpResult == null)
        {
            return (false, "请求服务器失败", null);
        }
        if (httpResult.status != 1 || httpResult.error_code != 0)
        {
            return (false, httpResult.error_msg, null);
        }

        var musics = new List<MusicSearchResult>();
        if (httpResult.data.lists.Length == 0)
        {
            return (true, "", new List<MusicSearchResult>());
        }
        foreach (var httpMusic in httpResult.data.lists)
        {
            try
            {
                var music = new MusicSearchResult()
                {
                    Id = MD5Utils.GetStringValueToLower($"{Platform}-{httpMusic.ID}"),
                    Platform = Platform,
                    PlatformInnerId = httpMusic.ID,
                    Name = KuGouUtils.RemoveSongNameTag(httpMusic.SongName),
                    Alias = "",
                    Artist = KuGouUtils.RemoveSongNameTag(httpMusic.SingerName),
                    Album = httpMusic.AlbumName,
                    Duration = TimeSpan.FromSeconds(httpMusic.Duration),
                    Fee = GetFeeFlag(httpMusic.Privilege),
                    PlatformData = new KuGouSearchExtendData()
                    {
                        Hash = httpMusic.FileHash,
                        AlbumId = httpMusic.AlbumID
                    }
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

    private FeeEnum GetFeeFlag(int Privilege)
    {
        if (Privilege == 10)
        {
            return FeeEnum.Demo;
        }
        return FeeEnum.Free;
    }

    public async Task<Music?> GetMusicDetailAsync(MusicSearchResult sourceMusic, MusicFormatTypeEnum musicFormatType)
    {

        if (!(sourceMusic.PlatformData is KuGouSearchExtendData extendData))
        {
            throw new ArgumentException("平台数据初始化异常");
        }
        string args = KuGouUtils.GetMusicUrlData(extendData.Hash, extendData.AlbumId);
        string url = $"{UrlBase.KuGou.GetMusic}?{args}";

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url)
        };
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (json.IsEmpty())
        {
            return null;
        }
        var httpResult = json.ToObject<HttpResultBase<MusicDetailHttpResult>>();
        if (httpResult == null)
        {
            return null;
        }
        if (httpResult.status != 1 || httpResult.error_code != 0)
        {
            return null;
        }

        string extendDataString = extendData.ToJson();

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
            ImageUrl = httpResult.data.img,
            PlayUrl = httpResult.data.play_url,
            Lyric = httpResult.data.lyrics,
            ExtendData = extendDataString
        };
    }

    public async Task<Music?> UpdatePlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType)
    {
        var extendDataString = music.ExtendData;
        if (extendDataString.IsEmpty())
        {
            Logger.Info("更新酷狗播放地址失败，扩展数据不存在");
            return music;
        }
        KuGouSearchExtendData? extendData;
        try
        {
            extendData = extendDataString.ToObject<KuGouSearchExtendData>();
            if (extendData == null)
            {
                Logger.Info("更新酷狗播放地址失败，扩展数据格式错误");
                return music;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("更新酷狗播放地址失败。", ex);
            return music;
        }

        string args = KuGouUtils.GetMusicUrlData(extendData.Hash, extendData.AlbumId);
        string url = $"{UrlBase.KuGou.GetMusic}?{args}";

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url)
        };
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
        request.Headers.Add("User-Agent", RequestHeaderBase.UserAgentEdge);
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (json.IsEmpty())
        {
            Logger.Info("更新酷狗播放地址失败，服务器返回空。");
            return music;
        }
        var httpResult = json.ToObject<HttpResultBase<MusicDetailHttpResult>>();
        if (httpResult == null)
        {
            Logger.Info("更新酷狗播放地址失败，服务器数据格式不正确。");
            return music;
        }
        if (httpResult.status != 1 || httpResult.error_code != 0)
        {
            Logger.Info("更新酷狗播放地址失败，服务器返回错误。");
            return music;
        }
        music.PlayUrl = httpResult.data.play_url;
        return music;
    }

    public Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>?> GetHotWordAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetMusicShareUrlAsync(Music music)
    {
        var obj = music.ExtendData.ToObject<KuGouSearchExtendData>();
        if (obj == null)
        {
            return Task.FromResult(UrlBase.KuGou.Index);
        }
        return Task.FromResult($"{UrlBase.KuGou.GetMusicPlayPage}/#hash={obj.Hash}&album_id={obj.AlbumId}&album_audio_id={music.PlatformInnerId}");
    }
}