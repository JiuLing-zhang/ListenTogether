using ListenTogether.Api.Models;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Api.Interfaces
{
    public interface IMyFavoriteService
    {
        Task<Result<MyFavoriteResponse>> GetOneAsync(int userId, int id);
        Task<List<MyFavoriteResponse>?> GetAllAsync(int userId);
        Task<Result> NameExist(int userId, string myFavoriteName);
        Task<Result<MyFavoriteResponse>> AddOrUpdateAsync(int userId, MyFavoriteRequest myFavorite);
        Task<Result> RemoveAsync(int userId, int id);

        Task<Result> AddMusicToMyFavorite(int userId, int id, MusicRequest music);
        Task<List<MyFavoriteDetailResponse>?> GetMyFavoriteDetail(int userId, int id);
        Task<Result> RemoveDetailAsync(int userId, int id);
    }
}
