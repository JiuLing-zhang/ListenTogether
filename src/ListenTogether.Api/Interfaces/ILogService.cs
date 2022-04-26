using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;

namespace MusicPlayerOnline.Api.Interfaces;
public interface ILogService
{
    public Task<Result> WriteAsync(int userId, LogRequest log);
    public Task<Result> WriteListAsync(int userId, List<LogRequest> logs);
}