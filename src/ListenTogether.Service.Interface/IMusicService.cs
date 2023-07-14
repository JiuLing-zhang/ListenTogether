using ListenTogether.Model;

namespace ListenTogether.Data.Interface;
public interface IMusicService
{
    Task<LocalMusic?> GetOneAsync(string id);
    Task<bool> AddOrUpdateAsync(LocalMusic music);
}