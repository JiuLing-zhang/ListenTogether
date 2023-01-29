using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;
public interface IMyFavoriteService
{
    Task<MyFavorite?> GetOneAsync(int id);
    Task<List<MyFavorite>> GetAllAsync();
    Task<MyFavorite?> AddOrUpdateAsync(MyFavorite myFavorite);
    Task<bool> NameExistAsync(string myFavoriteName);
    Task<bool> RemoveAsync(int id);
    Task<bool> AddMusicToMyFavoriteAsync(int id, string musicId);
    Task<List<MyFavoriteDetail>> GetMyFavoriteDetailAsync(int id);
    Task<bool> RemoveDetailAsync(int id);

    Task<List<string>> GetAllMusicIdAsync();
}