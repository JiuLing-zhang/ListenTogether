using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class PlaylistService : IPlaylistService
{
    public Task<bool> AddOrUpdateAsync(Playlist playlist)
    {
        throw new NotImplementedException();
    }

    public Task<List<Playlist>?> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAllAsync()
    {
        throw new NotImplementedException();
    }
}