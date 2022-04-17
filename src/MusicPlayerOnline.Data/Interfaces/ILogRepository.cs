using MusicPlayerOnline.Model;
namespace MusicPlayerOnline.Data.Interfaces;

public interface ILogRepository
{
    Task<bool> WriteListAsync(List<Log> logs);
}