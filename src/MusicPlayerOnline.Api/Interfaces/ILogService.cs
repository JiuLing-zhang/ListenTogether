using MusicPlayerOnline.Model.Api.Request;

namespace MusicPlayerOnline.Api.Interfaces;
public interface ILogService
{
    public Task WriteAsync(int userId, LogRequest log);
}