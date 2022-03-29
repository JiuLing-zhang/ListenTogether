using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;
public interface IMyFavoriteService
{
    Task<MyFavorite?> GetOneAsync(int id);
    Task<List<MyFavorite>?> GetAllAsync();
    Task<bool> AddOrUpdateAsync(MyFavorite myFavorite);
    Task<bool> RemoveAsync(int id);

    Task<bool> AddMusicToMyFavorite(int id, MyFavoriteDetail music);
    Task<List<MyFavoriteDetail>?> GetMyFavoriteDetail(int id);
}