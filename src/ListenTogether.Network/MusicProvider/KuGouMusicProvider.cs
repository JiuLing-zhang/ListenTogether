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

    public async Task<List<MusicResultShow>> SearchAsync(string keyword)
    {
        var musics = new List<MusicResultShow>();

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
            return musics;
        }

        HttpResultBase<HttpMusicSearchResult>? httpResult;
        try
        {
            httpResult = json.ToObject<HttpResultBase<HttpMusicSearchResult>>();
        }
        catch (Exception ex)
        {
            Logger.Error("解析酷狗搜索结果失败。", ex);
            return musics;
        }
        if (httpResult == null || httpResult.status != 1 || httpResult.error_code != 0)
        {
            return musics;
        }

        foreach (var httpMusic in httpResult.data.lists)
        {
            try
            {
                var ExtendDataJson = (new KuGouSearchExtendData()
                {
                    Hash = httpMusic.FileHash,
                    AlbumId = httpMusic.AlbumID
                }).ToJson();
                var music = new MusicResultShow()
                {
                    Id = MD5Utils.GetStringValueToLower($"{Platform}-{httpMusic.ID}"),
                    Platform = Platform,
                    IdOnPlatform = httpMusic.ID,
                    Name = KuGouUtils.RemoveSongNameTag(httpMusic.SongName),
                    Artist = KuGouUtils.RemoveSongNameTag(httpMusic.SingerName),
                    Album = httpMusic.AlbumName,
                    Duration = TimeSpan.FromSeconds(httpMusic.Duration),
                    Fee = GetFeeFlag(httpMusic.Privilege),
                    ExtendDataJson = ExtendDataJson
                };
                musics.Add(music);
            }
            catch (Exception ex)
            {
                Logger.Error("构建酷狗搜索结果失败。", ex);
            }
        }
        return musics;
    }

    private FeeEnum GetFeeFlag(int Privilege)
    {
        if (Privilege == 10)
        {
            return FeeEnum.Demo;
        }
        return FeeEnum.Free;
    }

    public Task<List<string>?> GetSearchSuggestAsync(string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<List<SongMenu>> GetSongMenusFromTop()
    {
        throw new NotImplementedException();
    }

    public Task<List<MusicResultShow>> GetTopMusicsAsync(string topId)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>?> GetHotWordAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetShareUrlAsync(string id, object? extendData = null)
    {
        var obj = extendData as KuGouSearchExtendData;
        if (obj == null)
        {
            return Task.FromResult(UrlBase.KuGou.Index);
        }
        return Task.FromResult($"{UrlBase.KuGou.GetMusicPlayPage}/#hash={obj.Hash}&album_id={obj.AlbumId}&album_audio_id={id}");
    }

    public async Task<string> GetLyricAsync(string id, object? extendData = null)
    {
        var data = extendData as KuGouSearchExtendData;
        if (data == null)
        {
            throw new ArgumentException("平台数据初始化异常");
        }
        string args = KuGouUtils.GetMusicUrlData(data.Hash, data.AlbumId);
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

        return httpResult.data.lyrics;
    }

    public Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<SongMenu>> GetSongMenusFromTagAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<MusicResultShow>> GetTagMusicsAsync(string tagId)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetPlayUrlAsync(string id, object? extendData = null)
    {
        throw new NotImplementedException();
    }
}