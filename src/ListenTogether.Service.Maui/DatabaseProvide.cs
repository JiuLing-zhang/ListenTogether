using JiuLing.CommonLibs.ExtensionMethods;
using ListenTogether.Data.Maui.Entities;
using SQLite;

namespace ListenTogether.Data.Maui;

public class DatabaseProvide
{
    private static string _dbPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ListenTogether"), "ListenTogether.db");

    private DatabaseProvide()
    {

    }

    public static void Initialize()
    {
        Database.CreateTable<MusicEntity>();
        Database.CreateTable<PlaylistEntity>();
        Database.CreateTable<MyFavoriteEntity>();
        Database.CreateTable<MyFavoriteDetailEntity>();
        Database.CreateTable<EnvironmentConfigEntity>();
        Database.CreateTable<UserEntity>();
        Database.CreateTable<MusicCacheEntity>();
    }

    private static SQLiteConnection? _database;
    public static SQLiteConnection Database
    {
        get
        {
            if (_database == null)
            {
                if (_dbPath.IsEmpty())
                {
                    throw new Exception("数据库路径未配置");
                }
                _database = new SQLiteConnection(_dbPath);
            }
            return _database;
        }
    }

    private static SQLiteAsyncConnection? _databaseAsync;
    public static SQLiteAsyncConnection DatabaseAsync
    {
        get
        {
            if (_databaseAsync == null)
            {
                if (_dbPath.IsEmpty())
                {
                    throw new Exception("数据库路径未配置");
                }
                _databaseAsync = new SQLiteAsyncConnection(_dbPath);

            }
            return _databaseAsync;
        }
    }    
}