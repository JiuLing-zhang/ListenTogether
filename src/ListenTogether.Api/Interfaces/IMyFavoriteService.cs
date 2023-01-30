using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Api.Interfaces
{
    public interface IMyFavoriteService
    {
        Task<Result<MyFavoriteResponse>> GetOneAsync(int userId, int id);
        Task<List<MyFavoriteResponse>> GetAllAsync(int userId);
        Task<Result> NameExistAsync(int userId, string myFavoriteName);
        Task<Result<MyFavoriteResponse>> AddOrUpdateAsync(int userId, MyFavoriteRequest myFavorite);
        Task<Result> RemoveAsync(int userId, int id);

        Task<Result> AddMusicToMyFavoriteAsync(int userId, int id, string musicId);
        Task<List<MyFavoriteDetailResponse>> GetMyFavoriteDetailAsync(int userId, int id);
        Task<Result> RemoveDetailAsync(int userId, int id);
    }
}
