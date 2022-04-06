namespace MusicPlayerOnline.Model.Api.Response
{
    public class MusicResponse : MusicBase
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
        public string PlatformInnerId { get; set; } = null!;
        /// <summary>
        /// 缓存地址
        /// </summary>
        public string CachePath { get; set; } = null!;
    }
}
