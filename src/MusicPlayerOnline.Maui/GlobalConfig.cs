using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Maui
{
    internal class GlobalConfig
    {
        /// <summary>
        /// App名称
        /// </summary>
        private const string AppName = "MusicPlayerOnline";

        /// <summary>
        /// App Data文件夹路径
        /// </summary>
        public static readonly string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);
        /// <summary>
        /// 歌曲缓存路径
        /// </summary>
        public static readonly string MusicCacheDirectory = Path.Combine(AppDataDirectory, "musics");

        public static EnvironmentSetting MyUserSetting { get; set; }
    }
}
