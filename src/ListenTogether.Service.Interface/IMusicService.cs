using ListenTogether.Model;

namespace ListenTogether.Data.Interface;
public interface IMusicRepository
{
    Task<LocalMusic?> GetOneAsync(string id);
    Task<bool> AddOrUpdateAsync(LocalMusic music);
}