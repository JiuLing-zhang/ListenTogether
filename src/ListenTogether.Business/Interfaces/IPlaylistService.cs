using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;
public interface IPlaylistService
{
    Task<bool> AddToPlaylistAsync(Playlist playlist);
    Task<Playlist?> GetOneAsync(string musicId);
    Task<List<Playlist>> GetAllAsync();
    Task<bool> RemoveAsync(int id);
    Task<bool> RemoveAllAsync();
}