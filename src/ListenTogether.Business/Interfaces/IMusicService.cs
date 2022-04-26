using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;
public interface IMusicService
{
    Task<Music?> GetOneAsync(string id);
    Task<bool> AddOrUpdateAsync(Music music);
}