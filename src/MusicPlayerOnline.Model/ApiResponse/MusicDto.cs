namespace MusicPlayerOnline.Model.ApiResponse
{
    public class MusicDto : MusicBase
    {
        /// <summary>
        /// 平台
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName { get; set; } = null!;
        /// <summary>
        /// 对应平台的ID
        /// </summary>
        public string PlatformId { get; set; } = null!;
        /// <summary>
        /// 缓存地址
        /// </summary>
        public string CachePath { get; set; } = null!;
    }
}
