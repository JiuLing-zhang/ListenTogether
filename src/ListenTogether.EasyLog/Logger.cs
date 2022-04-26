namespace ListenTogether.EasyLog;
public class Logger
{
    public static void Info(string msg)
    {
        try
        {
            var logEntity = new LogEntity()
            {
                CreateTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
                LogType = 0, //Info = 0,
                Message = msg
            };
            DatabaseProvide.Database.Insert(logEntity);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public static void Error(string msg, Exception ex)
    {
        try
        {
            var logEntity = new LogEntity()
            {
                CreateTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds(),
                LogType = 2, //Error = 2
                Message = $"{msg}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}"
            };
            DatabaseProvide.Database.Insert(logEntity);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    /// <summary>
    /// 获取所有日志
    /// </summary>
    public static List<(long CreateTime, int LogType, string Message)> GetAll(int maxCount = 30)
    {
        var result = new List<(long CreateTime, int LogType, string Message)>();
        var logs = DatabaseProvide.Database.Table<LogEntity>().OrderByDescending(x => x.CreateTime).Take(maxCount).ToList();
        foreach (var log in logs)
        {
            result.Add((log.CreateTime, log.LogType, log.Message));
        }
        return result;
    }

    /// <summary>
    /// 清空日志
    /// </summary>
    public static void RemoveAllAsync()
    {
        DatabaseProvide.Database.DeleteAll<LogEntity>();
    }
}