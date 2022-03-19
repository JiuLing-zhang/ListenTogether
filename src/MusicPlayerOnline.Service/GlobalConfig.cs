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
    }
}
