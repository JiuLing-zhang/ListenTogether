using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;
public interface IPlaylistService
{
    Task<bool> AddOrUpdateAsync(Playlist playlist);
    Task<List<Playlist>?> GetAllAsync();
    Task<bool> RemoveAsync(int id);
    Task<bool> RemoveAllAsync();
}