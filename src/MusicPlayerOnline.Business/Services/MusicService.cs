using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class MusicService : IMusicService
{
    private readonly IMusicRepository _repository;
    public MusicService(IMusicRepository repository)
    {
        _repository = repository;
    }

    public async Task<Music?> GetOneAsync(string id)
    {
        return await _repository.GetOneAsync(id);
    }

    public async Task<bool> AddOrUpdateAsync(Music music)
    {
        return await _repository.AddOrUpdateAsync(music);
    }

    public async Task<bool> UpdateCacheAsync(string id, string cachePath)
    {
        return await _repository.UpdateCacheAsync(id, cachePath);
    }
}