using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class MyFavoriteService : IMyFavoriteService
{
    private readonly IMyFavoriteRepository _repository;
    public MyFavoriteService(IMyFavoriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<MyFavorite?> GetOneAsync(int id)
    {
        return await _repository.GetOneAsync(id);
    }

    public async Task<List<MyFavorite>?> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<bool> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        return await _repository.AddOrUpdateAsync(myFavorite);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        return await _repository.RemoveAsync(id);
    }

    public async Task<bool> AddMusicToMyFavorite(int id, MyFavoriteDetail music)
    {
        return await _repository.AddMusicToMyFavorite(id, music);
    }

    public async Task<List<MyFavoriteDetail>?> GetMyFavoriteDetail(int id)
    {
        return await _repository.GetMyFavoriteDetail(id);
    }
}