using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Business.Interfaces;
public interface IMusicNetworkService
{
    Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword);
    Task<Music?> GetMusicDetail(MusicSearchResult musicSearchResult);
    Task<Music?> UpdatePlayUrl(Music music);
}