using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Repository.Repositories;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
public class MusicLocalService : IMusicService
{
    private readonly MusicRepository _repository;
    public MusicLocalService()
    {
        _repository = new MusicRepository();
    }

    public async Task<Result<MusicDto>> GetOneAsync(string id)
    {
        return await _repository.GetOneAsync(id);
    }

    public async Task<Result> AddOrUpdateAsync(Music music)
    {
        return await _repository.AddOrUpdateAsync(music);
    }

    public async Task<Result> UpdateCacheAsync(string id, string cachePath)
    {
        return await _repository.UpdateCacheAsync(id, cachePath);
    }
}