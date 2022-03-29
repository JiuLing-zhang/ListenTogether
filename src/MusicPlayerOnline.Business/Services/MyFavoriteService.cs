using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class MyFavoriteService : IMyFavoriteService
{
    public Task<MyFavorite?> GetOneAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<MyFavorite>?> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddMusicToMyFavorite(int id, MyFavoriteDetail music)
    {
        throw new NotImplementedException();
    }

    public Task<List<MyFavoriteDetail>?> GetMyFavoriteDetail(int id)
    {
        throw new NotImplementedException();
    }
}