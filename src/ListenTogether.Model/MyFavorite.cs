namespace MusicPlayerOnline.Model
{
    public class MyFavorite
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 展示的图片
        /// </summary>
        public string ImageUrl { get; set; } = null!;
        /// <summary>
        /// 歌曲总数
        /// </summary>
        public int MusicCount { get; set; }
    }
}
