using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data.Entities;
using SQLite;

namespace MusicPlayerOnline.Data;
internal class DatabaseProvide
{
    private static string _dbPath = "";
    public static void SetConnection(string dbPath)
    {
        _dbPath = dbPath;
        InitTable();
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
    private static void InitTable()
    {
        Database.CreateTable<MusicEntity>();
        Database.CreateTable<PlaylistEntity>();
        Database.CreateTable<MyFavoriteEntity>();
        Database.CreateTable<MyFavoriteDetailEntity>();
        Database.CreateTable<EnvironmentConfigEntity>();
        Database.CreateTable<UserEntity>();
    }
}