using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;

namespace MusicPlayerOnline.Service.Interfaces;
public interface IMusicNetworkService
{
    Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword);
    Task<Music?> GetMusicDetail(MusicSearchResult musicSearchResult);
    Task<Music?> UpdatePlayUrl(Music music);
}