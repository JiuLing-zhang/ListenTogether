using ListenTogether.Model;
namespace ListenTogether.Data.Interfaces;

public interface ILogRepository
{
    Task<bool> WriteListAsync(List<Log> logs);
}