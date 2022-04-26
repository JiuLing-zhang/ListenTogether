using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Business.Interfaces;
public interface IMusicNetworkService
{
    Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword);
    Task<Music?> GetMusicDetail(MusicSearchResult musicSearchResult);
    Task<Music?> UpdatePlayUrl(Music music);
}