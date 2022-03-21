using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Service.Interfaces;
public interface IMyFavoriteService
{
    Task<Result<MyFavoriteDto>> GetOneAsync(int id);
    Task<List<MyFavoriteDto>?> GetAllAsync();
    Task<Result> AddOrUpdateAsync(MyFavorite myFavorite);
    Task<Result> RemoveAsync(int id);

    Task<Result> AddMusicToMyFavorite(int id, MyFavoriteDetail music);
    Task<List<MyFavoriteDetailDto>?> GetMyFavoriteDetail(int id);
}