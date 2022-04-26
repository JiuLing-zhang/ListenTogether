using ListenTogether.Api.Models;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Api.Interfaces
{
    public interface IPlaylistService
    {
        Task<Result> AddOrUpdateAsync(int userId, PlaylistRequest playlist);
        Task<List<PlaylistResponse>> GetAllAsync(int userId);
        Task<Result> RemoveAsync(int userId, int id);
        Task<Result> RemoveAllAsync(int userId);
    }
}
