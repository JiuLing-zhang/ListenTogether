using System.ComponentModel.DataAnnotations;

namespace MusicPlayerOnline.Model.Api.Request
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class MyFavoriteRequest
    {
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 展示的图片
        /// </summary>
        public string ImageUrl { get; set; } = null!;
    }
}
