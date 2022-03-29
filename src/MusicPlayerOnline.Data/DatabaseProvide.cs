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
                InitTable();
            }
            return _databaseAsync;
        }
    }
    private static void InitTable()
    {
        DatabaseAsync.CreateTableAsync<MusicEntity>().Wait();
        DatabaseAsync.CreateTableAsync<PlaylistEntity>().Wait();
        DatabaseAsync.CreateTableAsync<MyFavoriteEntity>().Wait();
        DatabaseAsync.CreateTableAsync<MyFavoriteDetailEntity>().Wait();
        DatabaseAsync.CreateTableAsync<UserConfigEntity>().Wait();
       
        if (DatabaseAsync.Table<UserConfigEntity>().CountAsync().Result == 0)
        {
            DatabaseAsync.InsertAsync(new UserConfigEntity()).Wait();
        }

        if (DatabaseAsync.Table<EnvironmentConfigEntity>().CountAsync().Result == 0)
        {
            DatabaseAsync.InsertAsync(new EnvironmentConfigEntity()).Wait();
        }

        DatabaseAsync.CreateTableAsync<TokenEntity>().Wait();
        if (DatabaseAsync.Table<TokenEntity>().CountAsync().Result == 0)
        {
            DatabaseAsync.InsertAsync(new TokenEntity()).Wait();
        }


    }
}