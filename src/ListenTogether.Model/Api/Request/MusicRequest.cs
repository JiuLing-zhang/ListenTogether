using System.ComponentModel.DataAnnotations;

namespace ListenTogether.Model.Api.Request
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
        public string PlatformInnerId { get; set; } = null!;
        /// <summary>
        /// 扩展数据
        /// </summary>
        public string? ExtendData { get; set; }
    }
}
