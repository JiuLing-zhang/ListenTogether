using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Service.Interfaces;
internal interface IPlaylistService
{
    Task<Result> AddOrUpdateAsync(Playlist playlist);
    Task<List<PlaylistDto>?> GetAllAsync();
    Task<Result> RemoveAsync(int id);
    Task<Result> RemoveAllAsync();
}