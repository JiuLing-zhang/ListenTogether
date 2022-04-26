using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;
public interface IMusicRepository
{
    Task<Music?> GetOneAsync(string id);
    Task<bool> AddOrUpdateAsync(Music music);
}