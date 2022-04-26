using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;

public interface IApiLogService
{
    Task<bool> WriteListAsync(List<Log> logs);
}