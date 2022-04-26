using ListenTogether.Api.DbContext;
using ListenTogether.Api.Entities;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;

namespace ListenTogether.Api.Services;
public class LogService : ILogService
{
    private readonly DataContext _context;
    public LogService(DataContext dataContext)
    {
        _context = dataContext;
    }
    public async Task<Result> WriteAsync(int userId, LogRequest log)
    {
        var myLog = new LogEntity()
        {
            UserBaseId = userId,
            LogType = log.LogType.ToString(),
            Message = log.Message,
            LogTime = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.Timestamp),
            CreateTime = DateTime.Now
        };
        _context.Logs.Add(myLog);
        await _context.SaveChangesAsync();
        return new Result(0, "上传成功");
    }

    public async Task<Result> WriteListAsync(int userId, List<LogRequest> logs)
    {
        var createTime = DateTime.Now;
        foreach (var log in logs)
        {
            var myLog = new LogEntity()
            {
                UserBaseId = userId,
                LogType = log.LogType.ToString(),
                Message = log.Message,
                LogTime = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.Timestamp),
                CreateTime = createTime
            };
            _context.Logs.Add(myLog);
        }
        await _context.SaveChangesAsync();
        return new Result(0, "上传成功");
    }
}