using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface IMusicRepository
{
    Task<Music?> GetOneAsync(string id);
    Task<bool> AddOrUpdateAsync(Music music);
    Task<bool> UpdateCacheAsync(string id, string cachePath);
}