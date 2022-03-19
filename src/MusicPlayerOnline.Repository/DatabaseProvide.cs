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
                    if (string.IsNullOrEmpty(_dbPath))
                    {
                        throw new Exception("非法的数据库路径");
                    }

                    _databaseAsync = new SQLiteAsyncConnection(_dbPath);
                    InitTableAsync();
                }
                return _databaseAsync;
            }
        }
        private static void InitTableAsync()
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
        }
    }
}
