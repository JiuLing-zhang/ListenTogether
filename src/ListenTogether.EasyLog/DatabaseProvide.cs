using SQLite;
using System.Text.Json;

namespace ListenTogether.EasyLog;
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
                    string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ListenTogether");
                    if (!Directory.Exists(appDataFolder))
                    {
                        Directory.CreateDirectory(appDataFolder);
                    }
                    string dbPath = Path.Combine(appDataFolder, "ListenTogether.Log.db");

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