using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;
public interface IMusicRepository
{
    Task<LocalMusic?> GetOneAsync(string id);
    Task<bool> AddOrUpdateAsync(LocalMusic music);
}