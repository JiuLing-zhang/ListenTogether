using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;
public interface IMusicService
{
    Task<LocalMusic?> GetOneAsync(string id);
    Task<bool> AddOrUpdateAsync(LocalMusic music);
}