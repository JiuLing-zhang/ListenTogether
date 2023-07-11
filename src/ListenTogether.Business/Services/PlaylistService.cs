using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interface;
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
    public async Task<bool> AddToPlaylistAsync(List<Playlist> playlists)
    {
        return await _repository.AddOrUpdateAsync(playlists);
    }
    public async Task<Playlist?> GetOneAsync(string musicId)
    {
        return await _repository.GetOneAsync(musicId);
    }

    public async Task<List<Playlist>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<bool> RemoveAsync(string musicId)
    {
        return await _repository.RemoveAsync(musicId);
    }

    public async Task<bool> RemoveAllAsync()
    {
        return await _repository.RemoveAllAsync();
    }

}