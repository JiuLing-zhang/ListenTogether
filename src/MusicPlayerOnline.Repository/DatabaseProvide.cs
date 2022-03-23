using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Repository.Entities;
using SQLite;

namespace MusicPlayerOnline.Repository
{
    public class DatabaseProvide
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
            //配置表不存在时创建
            if (DatabaseAsync.Table<UserConfigEntity>().CountAsync().Result == 0)
            {
                DatabaseAsync.InsertAsync(new UserConfigEntity()).Wait();
            }

            DatabaseAsync.CreateTableAsync<TokenEntity>().Wait();
            if (DatabaseAsync.Table<TokenEntity>().CountAsync().Result == 0)
            {
                DatabaseAsync.InsertAsync(new TokenEntity()).Wait();
            }
        }
    }
}
