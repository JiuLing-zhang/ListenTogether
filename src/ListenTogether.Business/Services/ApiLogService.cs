using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;

public class ApiLogService : IApiLogService
{
    private readonly ILogRepository _repository;
    public ApiLogService(ILogRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> WriteListAsync(List<Log> logs)
    {
        return await _repository.WriteListAsync(logs);
    }
}