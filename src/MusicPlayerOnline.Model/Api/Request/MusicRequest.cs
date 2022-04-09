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
        public string PlatformInnerId { get; set; } = null!;
    }
}
