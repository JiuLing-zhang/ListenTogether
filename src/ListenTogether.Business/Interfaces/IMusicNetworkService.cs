using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Business.Interfaces;
public interface IMusicNetworkService
{
    Task<List<string>?> GetHotWordAsync();
    Task<List<string>?> GetSearchSuggestAsync(string keyword);
    Task<List<MusicResultShow>> SearchAsync(PlatformEnum platform, string keyword);
    Task<string> GetPlayUrlAsync(PlatformEnum platform, string id, object? extendData = null);
    Task<string?> GetLyricAsync(PlatformEnum platform, string id, object? extendData = null);
    Task<string> GetPlayPageUrlAsync(PlatformEnum platform, string id, object? extendData = null);
    Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync(PlatformEnum platform);
    Task<List<SongMenu>> GetSongMenusFromTagAsync(PlatformEnum platform, string id);
    Task<List<SongMenu>> GetSongMenusFromTop(PlatformEnum platform);
    Task<List<MusicResultShow>> GetTopMusicsAsync(PlatformEnum platform, string topId);
    Task<List<MusicResultShow>> GetTagMusicsAsync(PlatformEnum platform, string tagId);
}