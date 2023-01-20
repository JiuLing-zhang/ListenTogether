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
    Task<List<SongMenu>> GetSongMenusFromTagAsync(string id);

    /// <summary>
    /// 获取标签歌单详情
    /// </summary>
    /// <returns></returns>
    Task<List<MusicResultShow>> GetTagMusicsAsync(string tagId);

    /// <summary>
    /// 获取排行榜歌单
    /// </summary>
    /// <returns></returns>
    Task<List<SongMenu>> GetSongMenusFromTop();

    /// <summary>
    /// 获取排行榜歌单详情
    /// </summary>
    /// <returns></returns>
    Task<List<MusicResultShow>> GetTopMusicsAsync(string topId);

    Task<List<string>?> GetHotWordAsync();
    Task<List<string>?> GetSearchSuggestAsync(string keyword);
    Task<List<MusicResultShow>> SearchAsync(string keyword);
    //TODO 音质选择
    Task<string> GetPlayUrlAsync(string id, object? extendData = null);
    Task<string> GetLyricAsync(string id, object? extendData = null);
    Task<string> GetShareUrlAsync(string id, object? extendData = null);
}