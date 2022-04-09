using SQLite;
using System.Text.Json;

namespace MusicPlayerOnline.EasyLog;
internal class DatabaseProvide
{
    private static SQLiteConnection? _database;
    public static SQLiteConnection Database
    {
        get
        {
            if (_database == null)
            {
                try
                {
                    string configJson = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogConfig.json"));
                    var config = JsonSerializer.Deserialize<LogConfig>(configJson);
                    if (config == null)
                    {
                        throw new ArgumentException("日志组件加载失败：数据库路径未配置。");
                    }

                    string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MusicPlayerOnline");
                    if (!Directory.Exists(appDataFolder))
                    {
                        Directory.CreateDirectory(appDataFolder);
                    }
                    string dbPath = Path.Combine(appDataFolder, config.DbName);

                    _database = new SQLiteConnection(dbPath);
                    InitTable();

                }
                catch (Exception ex)
                {
                    throw new Exception($"日志组件加载失败：{ex.Message}");
                }
            }
            return _database;
        }
    }
    private static void InitTable()
    {
        Database.CreateTable<LogEntity>();
    }
}