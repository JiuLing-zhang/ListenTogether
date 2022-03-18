using System.ComponentModel.DataAnnotations;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Model.ApiRequest
{
    public class Music : MusicBase
    {
        /// <summary>
        /// 平台
        /// </summary>
        public PlatformEnum Platform { get; set; }

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
