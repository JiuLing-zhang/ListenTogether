namespace ListenTogether.Model.Api.Response
{
    /// <summary>
    /// 收藏的明细
    /// </summary>
    public class MyFavoriteDetailResponse
    {
        public int Id { get; set; }
        public int MyFavoriteId { get; set; }
        public int PlatformInt { get; set; }
        public string MusicId { get; set; } = null!;
        /// <summary>
        /// 歌曲名称
        /// </summary>
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
