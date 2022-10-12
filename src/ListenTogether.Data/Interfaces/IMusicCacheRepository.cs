using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;
public interface IMusicCacheRepository
{
    Task<MusicCache?> GetOneAsync(int id);
    Task<MusicCache?> GetOneByMuiscIdAsync(string musicId);
    Task<List<MusicCache>> GetAllAsync();
    Task<bool> AddOrUpdateAsync(string musicId, string fileName);
    Task<bool> RemoveAsync(int id);
}