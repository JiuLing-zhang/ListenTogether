using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;
public interface IPlaylistService
{
    Task<bool> AddToPlaylistAsync(Playlist playlist);
    Task<bool> AddToPlaylistAsync(List<Playlist> playlists);
    Task<Playlist?> GetOneAsync(string musicId);
    Task<List<Playlist>> GetAllAsync();
    Task<bool> RemoveAsync(string musicId);
    Task<bool> RemoveAllAsync();
}