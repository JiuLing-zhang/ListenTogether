using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Business.Interfaces;
public interface IMusicNetworkService
{
    Task<List<string>?> GetHotWordAsync();
    Task<List<string>?> GetSearchSuggestAsync(string keyword);
    Task<List<MusicSearchResult>> SearchAsync(PlatformEnum platform, string keyword);
    Task<Music?> GetMusicDetailAsync(MusicSearchResult musicSearchResult, MusicFormatTypeEnum musicFormatType);
    Task<string?> GetMusicPlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType);
    Task<string> GetMusicPlayPageUrlAsync(Music music);
}