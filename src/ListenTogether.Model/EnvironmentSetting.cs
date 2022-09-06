using ListenTogether.Model.Enums;

namespace ListenTogether.Model
{
    public class EnvironmentSetting
    {
        /// <summary>
        /// 播放器设置
        /// </summary>
        public PlayerSetting Player { get; set; } = null!;
        /// <summary>
        /// 常规设置
        /// </summary>
        public GeneralSetting General { get; set; } = null!;
        /// <summary>
        /// 平台设置
        /// </summary>
        public SearchSetting Search { get; set; } = null!;
        /// <summary>
        /// 播放设置
        /// </summary>
        public PlaySetting Play { get; set; } = null!;
    }

    public class PlayerSetting
    {
        /// <summary>
        /// 音量
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsSoundOff { get; set; }

        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayModeEnum PlayMode { get; set; }
    }

    public class GeneralSetting
    {
        /// <summary>
        /// 是否自动检查更新
        /// </summary>
        public bool IsAutoCheckUpdate { get; set; }

        /// <summary>
        /// 是否深色主题
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// 关闭时最小化到托盘
        /// </summary>
        public bool IsHideWindowWhenMinimize { get; set; }
    }

    public class SearchSetting
    {
        /// <summary>
        /// 启用的平台（PlatformEnum.Netease | PlatformEnum.KuGou | PlatformEnum.MiGu）
        /// </summary>
        public PlatformEnum EnablePlatform { get; set; }

        /// <summary>
        /// 隐藏小于1分钟的歌曲
        /// </summary>
        public bool IsHideShortMusic { get; set; }

        /// <summary>
        /// 歌曲名或歌手名必须包含搜索词
        /// </summary>
        public bool IsMatchSearchKey { get; set; }

        /// <summary>
        /// 隐藏收费歌曲
        /// </summary>
        public bool IsHideVipMusic { get; set; }
    }

    public class PlaySetting
    {
        /// <summary>
        /// 仅WIFI下可播放
        /// </summary>
        public bool IsWifiPlayOnly { get; set; }

        /// <summary>
        /// 播放页面禁止屏幕关闭
        /// </summary>
        public bool IsPlayingPageKeepScreenOn { get; set; }

        /// <summary>
        /// 添加到歌单时自动播放
        /// </summary>
        public bool IsPlayWhenAddToFavorite { get; set; }

        /// <summary>
        /// 播放我的歌单前清空播放列表
        /// </summary>
        public bool IsCleanPlaylistWhenPlayMyFavorite { get; set; }

        /// <summary>
        /// 歌曲无法播放时自动跳到下一首
        /// </summary>
        public bool IsAutoNextWhenFailed { get; set; }

        /// <summary>
        /// 音质
        /// </summary>
        public MusicFormatTypeEnum MusicFormatType { get; set; }
    }
}
