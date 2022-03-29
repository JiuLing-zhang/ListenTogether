using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Model
{
    public class EnvironmentSetting
    {
        public PlayerSetting Player { get; set; } = null!;
    }

    public class PlayerSetting
    {
        /// <summary>
        /// 音量
        /// </summary>
        public double Voice { get; set; }

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsSoundOff { get; set; }

        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayModeEnum PlayMode { get; set; }
    }
}
