using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistRepository _repository;
    public PlaylistService(IPlaylistRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> AddToPlaylistAsync(Playlist playlist)
    {
        return await _repository.AddOrUpdateAsync(playlist);
    }

    public async Task<List<Playlist>?> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        return await _repository.RemoveAsync(id);
    }

    public async Task<bool> RemoveAllAsync()
    {
        return await _repository.RemoveAllAsync();
    }
}