using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IMyFavoriteService
    {
        Task<Result<MyFavoriteDto>> GetOneAsync(int userId, int id);
        Task<List<MyFavoriteDto>?> GetAllAsync(int userId);


        //Task<Result<MyFavoriteDto>> GetMyFavoriteByName(string name);
        Task<Result> AddOrUpdateAsync(int userId, MyFavorite myFavorite);
        Task<Result> RemoveAsync(int userId, int id);

        Task<Result> AddMusicToMyFavorite(int userId, int id, MyFavoriteDetail music);
        Task<List<MyFavoriteDetailDto>?> GetMyFavoriteDetail(int userId, int id);
    }
}
