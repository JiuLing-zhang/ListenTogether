using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Repository.Repositories;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
public class PlaylistLocalService : IPlaylistService
{
    private readonly PlaylistRepository _repository;
    public PlaylistLocalService()
    {
        _repository = new PlaylistRepository();
    }

    public async Task<Result> AddOrUpdateAsync(Playlist playlist)
    {
        return await _repository.AddOrUpdateAsync(playlist);
    }

    public async Task<List<PlaylistDto>?> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Result> RemoveAsync(int id)
    {
        return await _repository.RemoveAsync(id);
    }

    public async Task<Result> RemoveAllAsync()
    {
        return await _repository.RemoveAllAsync();
    }
}