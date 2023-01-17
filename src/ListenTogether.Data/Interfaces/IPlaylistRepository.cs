using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;
public interface IPlaylistRepository
{
    Task<bool> AddOrUpdateAsync(Playlist playlist);
    Task<Playlist?> GetOneAsync(string musicId);
    Task<List<Playlist>> GetAllAsync();
    Task<bool> RemoveAsync(int musicId);
    Task<bool> RemoveAllAsync();
}