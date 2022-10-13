using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;
public class MusicCacheService : IMusicCacheService
{
    private readonly IMusicCacheRepository _repository;
    public MusicCacheService(IMusicCacheRepository repository)
    {
        _repository = repository;
    }
    public async Task<MusicCache?> GetOneAsync(int id)
    {
        return await _repository.GetOneAsync(id);
    }
    public async Task<MusicCache?> GetOneByMuiscIdAsync(string musicId)
    {
        return await _repository.GetOneByMuiscIdAsync(musicId);
    }

    public async Task<List<MusicCache>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<bool> AddOrUpdateAsync(string musicId, string fileName, string remark)
    {
        return await _repository.AddOrUpdateAsync(musicId, fileName, remark);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        return await _repository.RemoveAsync(id);
    }
}