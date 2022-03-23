using MusicPlayerOnline.Model;
using MusicPlayerOnline.Repository;

namespace MusicPlayerOnline.Service
{
    public class GlobalConfig
    {
        public static void SetDbConnection(string dbPath)
        {
            DatabaseProvide.SetConnection(dbPath);
        }
        public static void SetWebApi(string baseUrl)
        {
            ApiSetting = new ApiSettings(baseUrl);
        }

        /// <summary>
        /// API 的一些配置信息
        /// </summary>
        public static ApiSettings ApiSetting { get; set; } = null!;

        public static UserInfo? CurrentUser { get; set; }
        public static bool IsLogin => CurrentUser != null;
    }
}
