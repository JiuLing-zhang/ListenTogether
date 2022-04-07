using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;
public interface IPlaylistService
{
    Task<bool> AddToPlaylist(Playlist playlist);
    Task<List<Playlist>?> GetAllAsync();
    Task<bool> RemoveAsync(int id);
    Task<bool> RemoveAllAsync();
}