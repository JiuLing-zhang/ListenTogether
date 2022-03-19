using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IPlaylistService
    {
        Task<Result> AddOrUpdateAsync(int userId, Playlist playlist);
        Task<List<PlaylistDto>> GetAllAsync(int userId);
        Task<Result> RemoveAsync(int userId, int id);
        Task<Result> RemoveAllAsync(int userId);
    }
}
