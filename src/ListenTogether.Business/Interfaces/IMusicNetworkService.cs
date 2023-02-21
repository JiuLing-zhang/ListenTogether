using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Business.Interfaces;
public interface IMusicNetworkService
{
    void SetMusicFormatType(MusicFormatTypeEnum musicFormatType);
    Task<List<string>> GetHotWordAsync();
    Task<List<string>> GetSearchSuggestAsync(string keyword);
    Task<List<MusicResultShow>> SearchAsync(PlatformEnum platform, string keyword);
    Task<string> GetPlayUrlAsync(PlatformEnum platform, string id, string extendDataJson = "");
    Task<string> GetImageUrlAsync(PlatformEnum platform, string id, string extendDataJson = "");
    Task<string> GetLyricAsync(PlatformEnum platform, string id, string extendDataJson = "");
    Task<string> GetPlayPageUrlAsync(PlatformEnum platform, string id, string extendDataJson = "");
    Task<(List<MusicTag> HotTags, List<MusicTypeTag> AllTypes)> GetMusicTagsAsync(PlatformEnum platform);
    Task<List<SongMenu>> GetSongMenusFromTagAsync(PlatformEnum platform, string id, int page);
    Task<List<SongMenu>> GetSongMenusFromTop(PlatformEnum platform);
    Task<List<MusicResultShow>> GetTopMusicsAsync(PlatformEnum platform, string topId);
    Task<List<MusicResultShow>> GetTagMusicsAsync(PlatformEnum platform, string tagId);
}