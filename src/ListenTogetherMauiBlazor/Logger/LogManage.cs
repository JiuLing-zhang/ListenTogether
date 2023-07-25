using ListenTogether.Model;
using ListenTogether.Service.Interface;

namespace ListenTogetherMauiBlazor.Logger;
public class LogManage : ILogManage
{
    public Task<List<Log>> GetAllAsync()
    {
        var result = new List<Log>();
        var logs = LogDatabaseProvide.Database.Table<LogEntity>().OrderByDescending(x => x.CreateTime).Take(200).ToList();
        foreach (var log in logs)
        {
            result.Add(
                new Log()
                {
                    Timestamp = log.CreateTime,
                    LogType = log.LogType,
                    Message = log.Message
                });
        }
        return Task.FromResult(result);
    }

    public Task RemoveAllAsync()
    {
        LogDatabaseProvide.Database.DeleteAll<LogEntity>();
        return Task.CompletedTask;
    }
}