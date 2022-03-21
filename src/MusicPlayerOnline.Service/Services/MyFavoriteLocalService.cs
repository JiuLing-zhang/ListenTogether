using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Repository.Repositories;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
internal class MyFavoriteLocalService : IMyFavoriteService
{
    private readonly MyFavoriteRepository _repository;
    public MyFavoriteLocalService()
    {
        _repository = new MyFavoriteRepository();
    }
    public async Task<Result<MyFavoriteDto>> GetOneAsync(int id)
    {
        return await _repository.GetOneAsync(id);
    }

    public async Task<List<MyFavoriteDto>?> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Result> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        return await _repository.AddOrUpdateAsync(myFavorite);
    }

    public async Task<Result> RemoveAsync(int id)
    {
        return await _repository.RemoveAsync(id);
    }

    public async Task<Result> AddMusicToMyFavorite(int id, MyFavoriteDetail music)
    {
        return await _repository.AddMusicToMyFavorite(id, music);
    }

    public async Task<List<MyFavoriteDetailDto>?> GetMyFavoriteDetail(int id)
    {
        return await _repository.GetMyFavoriteDetail(id);
    }
}