using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Interfaces;
public interface ILogService
{
    public Task WriteAsync(int userId, Log log);
}