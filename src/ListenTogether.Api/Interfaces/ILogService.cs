using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;

namespace ListenTogether.Api.Interfaces;
public interface ILogService
{
    public Task<Result> WriteAsync(int userId, LogRequest log);
    public Task<Result> WriteListAsync(int userId, List<LogRequest> logs);
}