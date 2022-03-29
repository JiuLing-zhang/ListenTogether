using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class MusicService : IMusicService
{
    public Task<Music?> GetOneAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddOrUpdateAsync(Music music)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateCacheAsync(string id, string cachePath)
    {
        throw new NotImplementedException();
    }
}