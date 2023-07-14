using ListenTogether.Model;

namespace ListenTogether.Data.Interface;
public interface IPlaylistService
{
    Task<bool> AddOrUpdateAsync(Playlist playlist);
    Task<bool> AddOrUpdateAsync(List<Playlist> playlists);
    Task<Playlist?> GetOneAsync(string musicId);
    Task<List<Playlist>> GetAllAsync();
    Task<bool> RemoveAsync(string musicId);
    Task<bool> RemoveAllAsync();
}