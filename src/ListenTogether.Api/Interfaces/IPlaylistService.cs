using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IPlaylistService
    {
        Task<Result> AddOrUpdateAsync(int userId, PlaylistRequest playlist);
        Task<List<PlaylistResponse>> GetAllAsync(int userId);
        Task<Result> RemoveAsync(int userId, int id);
        Task<Result> RemoveAllAsync(int userId);
    }
}
