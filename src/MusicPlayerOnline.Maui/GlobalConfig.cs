using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Maui
{
    internal class GlobalConfig
    {
        /// <summary>
        /// App名称
        /// </summary>
        private static string AppName { get; set; } = AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// App Data文件夹路径
        /// </summary>
        public static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

        public static EnvironmentSetting MyUserSetting { get; set; }
    }
}
