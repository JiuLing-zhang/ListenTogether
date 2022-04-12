using JiuLing.CommonLibs.ExtensionMethods;
using JiuLing.CommonLibs.Security;
using MusicPlayerOnline.EasyLog;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Network.Models.NetEase;
using MusicPlayerOnline.Network.Utils;
using System.Text.Json;

namespace MusicPlayerOnline.Network.MusicProvider;
public class NetEaseMusicProvider : IMusicProvider
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const PlatformEnum Platform = PlatformEnum.NetEase;

    public async Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> Search(string keyword)
    {
        string url = $"{UrlBase.NetEase.Search}";

        var postData = NetEaseUtils.GetPostDataForSearch(keyword);
        var form = new FormUrlEncodedContent(postData);
        var response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        ResultBase<MusicSearchHttpResult>? result;
        try
        {
            result = json.ToObject<ResultBase<MusicSearchHttpResult>>();
        }
        catch (Exception ex)
        {
            Logger.Error("解析网易搜索结果失败。", ex);
            return (false, "解析数据失败", null);
        }

        if (result == null)
        {
            return (false, "请求服务器失败", null);
        }
        if (result.code != 200)
        {
            return (false, result.msg, null);
        }

        var musics = new List<MusicSearchResult>();
        if (result.result.songCount == 0)
        {
            return (true, "", new List<MusicSearchResult>());
        }
        foreach (var song in result.result.songs)
        {
            try
            {
                var ts = TimeSpan.FromMilliseconds(song.dt);

                string alia = "";
                if (song.alia.Length > 0)
                {
                    alia = song.alia[0];
                }

                string artistName = "";
                if (song.ar.Length > 0)
                {
                    artistName = string.Join("、", song.ar.Select(x => x.name).ToList());
                }
                var music = new MusicSearchResult()
                {
                    Id = MD5Utils.GetStringValueToLower($"{Platform}-{song.id}"),
                    Platform = Platform,
                    PlatformInnerId = song.id.ToString(),
                    Name = song.name,
                    Alias = alia,
                    Artist = artistName,
                    Album = song.al.name,
                    ImageUrl = song.al.picUrl,
                    Duration = song.dt,
                    DurationText = $"{ts.Minutes}:{ts.Seconds:D2}",
                    PlatformData = new SearchResultExtended()
                    {
                        Fee = song.fee
                    }
                };
                musics.Add(music);
            }
            catch (Exception ex)
            {
                Logger.Error("构建网易搜索结果失败。", ex);
            }

        }
        return (true, "", musics);
    }

    public async Task<Music?> GetMusicDetail(MusicSearchResult sourceMusic)
    {
        string url = $"{UrlBase.NetEase.GetMusic}";
        var postData = NetEaseUtils.GetPostDataForMusicUrl(sourceMusic.PlatformInnerId);

        var form = new FormUrlEncodedContent(postData);
        var response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var httpResult = json.ToObject<ResultBase<MusicUrlHttpResult>>();
        if (httpResult == null)
        {
            return null;
        }
        if (httpResult.code != 200)
        {
            return null;
        }

        if (httpResult.data.Count == 0)
        {
            return null;
        }

        string playUrl = httpResult.data[0].url;
        if (playUrl.IsEmpty())
        {
            return null;
        }

        //获取歌词
        url = $"{UrlBase.NetEase.Lyric}";
        postData = NetEaseUtils.GetPostDataForLyric(sourceMusic.PlatformInnerId);

        form = new FormUrlEncodedContent(postData);
        response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
        json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var lyricResult = json.ToObject<MusicLyricHttpResult>();
        if (lyricResult == null)
        {
            return null;
        }
        if (lyricResult.code != 200)
        {
            return null;
        }

        if (lyricResult.lrc == null)
        {
            return null;
        }

        return new Music()
        {
            Id = sourceMusic.Id,
            Platform = sourceMusic.Platform,
            PlatformInnerId = sourceMusic.PlatformInnerId,
            Name = sourceMusic.Name,
            Alias = sourceMusic.Alias,
            Artist = sourceMusic.Artist,
            Album = sourceMusic.Album,
            Duration = sourceMusic.Duration,
            ImageUrl = sourceMusic.ImageUrl,
            PlayUrl = playUrl,
            Lyric = lyricResult.lrc.lyric
        };
    }

    public async Task<Music?> UpdatePlayUrl(Music music)
    {
        string url = $"{UrlBase.NetEase.GetMusic}";
        var postData = NetEaseUtils.GetPostDataForMusicUrl(music.PlatformInnerId);

        var form = new FormUrlEncodedContent(postData);
        var response = await _httpClient.PostAsync(url, form).ConfigureAwait(false);
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var httpResult = json.ToObject<ResultBase<MusicUrlHttpResult>>();
        if (httpResult == null)
        {
            return null;
        }
        if (httpResult.code != 200)
        {
            return null;
        }

        if (httpResult.data.Count == 0)
        {
            return null;
        }

        string playUrl = httpResult.data[0].url;
        if (playUrl.IsEmpty())
        {
            return null;
        }

        music.PlayUrl = playUrl;
        return music;
    }
}
