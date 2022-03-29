using System.ComponentModel.DataAnnotations;

namespace MusicPlayerOnline.Model.Api.Request
{
    public class MusicRequest : MusicBase
    {
        /// <summary>
        /// 平台
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 对应平台的ID
        /// </summary>
        [Required]
        public string PlatformId { get; set; } = null!;
        /// <summary>
        /// 缓存地址
        /// </summary>
        [Required]
        public string CachePath { get; set; } = null!;
    }
}
