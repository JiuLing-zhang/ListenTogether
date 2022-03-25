using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;
using MusicPlayerOnline.Network;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
public class MusicNetworkService : IMusicNetworkService
{
    private readonly MusicNetPlatform _musicNetPlatform = new MusicNetPlatform();
    public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
    {
        return await _musicNetPlatform.Search(platform, keyword);
    }

    public async Task<Music?> GetMusicDetail(MusicSearchResult musicSearchResult)
    {
        return await _musicNetPlatform.GetMusicDetail(musicSearchResult);
    }

    public async Task<Music?> UpdatePlayUrl(Music music)
    {
        return await _musicNetPlatform.UpdatePlayUrl(music);
    }
}