namespace MusicPlayerOnline.Model
{
    public class MusicBase
    {
        /// <summary>
        /// ID，系统中唯一，guid.ToString("N")
        /// </summary>
        public string Id { get; set; } = null!;
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; } = null!;
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string Artist { get; set; } = null!;
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string Album { get; set; } = null!;
        /// <summary>
        /// 歌曲时长（毫秒）
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; } = null!;
        /// <summary>
        /// 播放地址
        /// </summary>
        public string PlayUrl { get; set; } = null!;
        /// <summary>
        /// 歌词
        /// </summary>
        public string Lyric { get; set; } = null!;
    }
}
