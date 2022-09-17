using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Network.MusicProvider;
public interface IMusicProvider
{
    Task<List<string>?> GetHotWordAsync();
    Task<List<string>?> GetSearchSuggestAsync(string keyword);
    Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> SearchAsync(string keyword);
    Task<Music?> GetMusicDetailAsync(MusicSearchResult sourceMusic, MusicFormatTypeEnum musicFormatType);
    Task<string?> GetMusicPlayUrlAsync(Music music, MusicFormatTypeEnum musicFormatType);
    Task<string> GetMusicShareUrlAsync(Music music);
}