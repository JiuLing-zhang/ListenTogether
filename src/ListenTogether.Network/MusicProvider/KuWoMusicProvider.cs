using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Enums;
using ListenTogether.Network.Models.KuWo;
using System.Text;

namespace ListenTogether.Network.MusicProvider;
internal class KuWoMusicProvider : IMusicProvider
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const PlatformEnum Platform = PlatformEnum.KuWo;
    public Task<List<string>> GetSearchSuggest(string keyword)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> Search(string keyword)
    {
        string url = $"{UrlBase.KuWo.Search}?key={keyword}";
        string json = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
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
            Logger.Error("解析酷我音乐搜索结果失败。", ex);
            return (false, "解析数据失败", null);
        }
        if (httpResult == null)
        {
            return (false, "请求服务器失败", null);
        }
        if (httpResult.success != true || httpResult.code != 200)
        {
            return (false, httpResult.message, null);
        }

        var musics = new List<MusicSearchResult>();
        if (httpResult.data.music.Length == 0)
        {
            return (true, "", new List<MusicSearchResult>());
        }

        foreach (var httpMusic in httpResult.data.music)
        {
            try
            {
                var music = new MusicSearchResult()
                {
                    Id = MD5Utils.GetStringValueToLower($"{Platform}-{httpMusic.id}"),
                    Platform = Platform,
                    PlatformInnerId = httpMusic.id.ToString(),
                    Name = httpMusic.name.Replace("&nbsp;", " "),
                    Alias = "",
                    Artist = httpMusic.artist_name.Replace("&nbsp;", " "),
                    Album = httpMusic.album_name.Replace("&nbsp;", " "),
                    Fee = GetFeeFlag(httpMusic.pay_info.listen_fragment)
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

    public async Task<Music?> GetMusicDetail(MusicSearchResult sourceMusic)
    {
        string musicId = sourceMusic.PlatformInnerId;
        //换播放地址
        string url = $"{UrlBase.KuWo.GetMusicUrl}?format=mp3&rid={musicId}&response=url&type=convert_url";
        string playUrl = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
        if (playUrl.IsEmpty())
        {
            Logger.Error("酷我播放地址获取失败。", new Exception($"服务器返回空，ID:{musicId}"));
            return null;
        }

        if (!JiuLing.CommonLibs.Text.RegexUtils.IsMatch(playUrl, "http\\S*\\.mp3"))
        {
            Logger.Error("酷我播放地址获取失败。", new Exception($"服务器返回：{playUrl}，ID:{musicId}"));
            return null;
        }

        //获取歌曲详情
        url = $"{UrlBase.KuWo.GetMusicDetail}/{musicId}";
        string json = await _httpClient.GetStringAsync(url).ConfigureAwait(false);
        if (json.IsEmpty())
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回空，ID:{musicId}"));
            return null;
        }

        HttpResultBase<MusicDetailHttpResult>? httpResult;
        try
        {
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
        if (httpResult.success != true || httpResult.code != 200)
        {
            Logger.Error("酷我歌曲详情获取失败。", new Exception($"服务器返回状态异常：{httpResult.message ?? ""}，ID:{musicId}"));
            return null;
        }

        //处理歌词
        var sbLyrics = new StringBuilder();
        if (httpResult.data.lrc != null && httpResult.data.lrc.Length > 0)
        {
            foreach (var lyricInfo in httpResult.data.lrc)
            {
                var ts = TimeSpan.FromSeconds(Convert.ToDouble(lyricInfo.time));
                string minutes = ts.Minutes.ToString();
                string seconds = ts.Seconds.ToString();
                string milliseconds = ts.Milliseconds.ToString();
                string time = $"[{minutes.PadLeft(2, '0')}:{seconds.PadLeft(2, '0')}.{milliseconds.PadLeft(3, '0')}]";
                sbLyrics.AppendLine($"{time}{lyricInfo.lineLyric}");
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
            Album = sourceMusic.Album,
            ImageUrl = httpResult.data.info.pic,
            PlayUrl = playUrl,
            Lyric = sbLyrics.ToString(),
            ExtendData = ""
        };
    }
    public Task<Music?> UpdatePlayUrl(Music music)
    {
        throw new NotImplementedException();
    }
}