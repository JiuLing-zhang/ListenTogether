namespace MusicPlayerOnline.EasyLog;
public class Logger
{
    public static void Info(string msg)
    {
        try
        {
            var logEntity = new LogEntity()
            {
                CreateTime = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds(),
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
                CreateTime = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds(),
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
}