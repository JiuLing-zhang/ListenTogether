using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface IMyFavoriteRepository
{
    Task<MyFavorite?> GetOneAsync(int id);
    Task<List<MyFavorite>?> GetAllAsync();
    Task<bool> AddOrUpdateAsync(MyFavorite myFavorite);
    Task<bool> RemoveAsync(int id);

    Task<bool> AddMusicToMyFavorite(int id, MyFavoriteDetail music);
    Task<List<MyFavoriteDetail>?> GetMyFavoriteDetail(int id);
}