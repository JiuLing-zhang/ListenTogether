using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Business.Interfaces;
public interface IMusicNetworkService
{
    Task<List<string>?> GetHotWordAsync();
    Task<List<string>?> GetSearchSuggestAsync(string keyword);
    Task<List<MusicSearchResult>> SearchAsync(PlatformEnum platform, string keyword);
    Task<Music?> GetDetailAsync(MusicSearchResult musicSearchResult, MusicFormatTypeEnum musicFormatType);
    Task<string?> GetPlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType);
    Task<string?> GetLyricAsync(Music music);
    Task<string> GetPlayPageUrlAsync(Music music);
    Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync(PlatformEnum platform);
    Task<List<SongMenu>> GetSongMenusFromTagAsync(PlatformEnum platform, string id);
    Task<List<SongMenu>> GetSongMenusFromTop(PlatformEnum platform);
    Task<List<MusicResultShow>> GetTopMusicsAsync(PlatformEnum platform, string topId);
    Task<List<MusicResultShow>> GetTagMusicsAsync(PlatformEnum platform, string tagId);
}