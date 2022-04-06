using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface IMyFavoriteRepository
{
    Task<MyFavorite?> GetOneAsync(int id);
    Task<List<MyFavorite>?> GetAllAsync();

    Task<MyFavorite?> AddOrUpdateByNameAsync(MyFavorite myFavorite);
    Task<bool> RemoveAsync(int id);

    Task<bool> AddMusicToMyFavorite(int id, Music music);
    Task<List<MyFavoriteDetail>?> GetMyFavoriteDetail(int id);
}