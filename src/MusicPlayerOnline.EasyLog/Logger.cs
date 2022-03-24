namespace MusicPlayerOnline.EasyLog;
public class Logger
{
    private static readonly string AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MusicPlayerOnline");
    public static void Info(string msg)
    {
        try
        {
            if (!Directory.Exists(AppDataFolder))
            {
                Directory.CreateDirectory(AppDataFolder);
            }
            DateTime now = DateTime.Now;
            string fileName = Path.Combine(AppDataFolder, $"Info-{now:yyyy-MM-dd}.log");
            string log = $"{now:yyyy-MM-dd HH:mm:ss.fff} {Environment.NewLine}{msg}{Environment.NewLine}";
            using var sw = new StreamWriter(fileName, true, System.Text.Encoding.UTF8);
            sw.Write(log);
            sw.Close();
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
            if (!Directory.Exists(AppDataFolder))
            {
                Directory.CreateDirectory(AppDataFolder);
            }
            DateTime now = DateTime.Now;
            string fileName = Path.Combine(AppDataFolder, $"Error-{now:yyyy-MM-dd}.log");
            string log = $"{Environment.NewLine}--- {now:yyyy-MM-dd HH:mm:ss.fff} ---{Environment.NewLine}{msg}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
            using var sw = new StreamWriter(fileName, true, System.Text.Encoding.UTF8);
            sw.Write(log);
            sw.Close();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}