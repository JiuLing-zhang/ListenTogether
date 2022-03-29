using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IMyFavoriteService
    {
        Task<Result<MyFavoriteResponse>> GetOneAsync(int userId, int id);
        Task<List<MyFavoriteResponse>?> GetAllAsync(int userId);


        //Task<Result<MyFavoriteDto>> GetMyFavoriteByName(string name);
        Task<Result> AddOrUpdateAsync(int userId, MyFavoriteRequest myFavorite);
        Task<Result> RemoveAsync(int userId, int id);

        Task<Result> AddMusicToMyFavorite(int userId, int id, MyFavoriteDetailRequest music);
        Task<List<MyFavoriteDetailResponse>?> GetMyFavoriteDetail(int userId, int id);
    }
}
