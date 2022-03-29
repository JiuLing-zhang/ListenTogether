using System.ComponentModel.DataAnnotations;

namespace MusicPlayerOnline.Model.Api.Request
{
    /// <summary>
    /// 收藏的明细
    /// </summary>
    public class MyFavoriteDetailRequest
    {
        public int MyFavoriteId { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public int Platform { get; set; }

        [Required]
        public string MusicId { get; set; } = null!;
        /// <summary>
        /// 歌曲名称
        /// </summary>
        [Required]
        public string MusicName { get; set; } = null!;
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string MusicArtist { get; set; } = null!;
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string MusicAlbum { get; set; } = null!;
    }
}
