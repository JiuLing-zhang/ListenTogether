using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;

public class MusicService : IMusicService
{
    private readonly IMusicRepository _repository;
    public MusicService(IMusicRepository repository)
    {
        _repository = repository;
    }

    public async Task<LocalMusic?> GetOneAsync(string id)
    {
        return await _repository.GetOneAsync(id);
    }

    public async Task<bool> AddOrUpdateAsync(LocalMusic music)
    {
        return await _repository.AddOrUpdateAsync(music);
    }
}