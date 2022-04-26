using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;

public interface IApiLogService
{
    Task<bool> WriteListAsync(List<Log> logs);
}