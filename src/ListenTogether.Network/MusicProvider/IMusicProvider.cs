using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Network.MusicProvider;
public interface IMusicProvider
{
    /// <summary>
    /// 获取音乐标签分类
    /// </summary>
    /// <returns></returns>
    Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync();

    /// <summary>
    /// 获取音乐标签对应的歌单
    /// </summary>
    /// <returns></returns>
    Task<List<MusicTagPlaylist>> GetMusicTagPlaylistAsync(string musicTagId);

    Task<List<string>?> GetHotWordAsync();
    Task<List<string>?> GetSearchSuggestAsync(string keyword);
    Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> SearchAsync(string keyword);
    Task<Music?> GetDetailAsync(MusicSearchResult sourceMusic, MusicFormatTypeEnum musicFormatType);
    Task<string?> GetPlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType);
    Task<string?> GetLyricAsync(Music music);
    Task<string> GetShareUrlAsync(Music music);
}