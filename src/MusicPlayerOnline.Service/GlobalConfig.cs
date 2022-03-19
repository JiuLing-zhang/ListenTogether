using MusicPlayerOnline.Repository;

namespace MusicPlayerOnline.Service
{
    public class GlobalConfig
    {
        /// <summary>
        /// API 的一些配置信息
        /// </summary>
        public static ApiSettings ApiSetting { get; set; } = null!;
        public static UserInfo? CurrentUser { get; set; }

        public static void SetWebApi(string baseUrl)
        {
            ApiSetting = new ApiSettings(baseUrl);
        }

        public static void SetDbConnection(string dbPath)
        {
            DatabaseProvide.SetConnection(dbPath);
        }

        public static void SetCurrentUser(string userName, string nickname, string avatar, string token, string refreshToken)
        {
            CurrentUser = new UserInfo(userName, nickname, avatar, token, refreshToken);
        }
    }
}
