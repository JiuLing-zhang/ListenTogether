using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Business.Services;

public class MusicNetworkService : IMusicNetworkService
{
    public Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<Music?> GetMusicDetail(MusicSearchResult musicSearchResult)
    {
        throw new NotImplementedException();
    }

    public Task<Music> UpdatePlayUrl(Music music)
    {
        throw new NotImplementedException();
    }
}