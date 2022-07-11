using ListenTogether.Model.Enums;

namespace ListenTogether
{
    internal class GlobalConfig
    {
        /// <summary>
        /// App名称
        /// </summary>
        private const string AppName = "ListenTogether";
        /// <summary>
        /// 本地数据库名
        /// </summary>
        public const string LocalDatabaseName = "ListenTogether.db";
        /// <summary>
        /// App Data文件夹路径
        /// </summary>
        public static readonly string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);
        /// <summary>
        /// 歌曲缓存路径
        /// </summary>
        public static readonly string MusicCacheDirectory = Path.Combine(AppDataDirectory, "musics");

        public static Version CurrentVersion { get; set; }
        public static string CurrentVersionString => CurrentVersion.ToString();

        public static EnvironmentSetting MyUserSetting { get; set; }
        public static AppSettings AppSettings { get; set; }

        /// <summary>
        /// 程序网络版本类型
        /// </summary>
        internal static AppNetworkEnum AppNetwork
        {
            get
            {
                if (AppSettings.ApiDomain.IsEmpty())
                {
                    return AppNetworkEnum.Standalone;
                }
                return AppNetworkEnum.Online;
            }
        }

        private static User? _currentUser;
        /// <summary>
        /// 当前用户
        /// </summary>
        public static User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                if (value == null)
                {
                    BusinessConfig.UserToken = null;
                }
                else
                {
                    BusinessConfig.UserToken = new TokenInfo()
                    {
                        Token = value.Token,
                        RefreshToken = value.RefreshToken
                    };
                }
            }
        }
    }
}
