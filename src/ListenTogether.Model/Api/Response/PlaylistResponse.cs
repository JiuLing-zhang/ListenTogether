namespace ListenTogether.Model.Api.Response
{
    /// <summary>
    /// 播放列表
    /// </summary>
    public class PlaylistResponse
    {
        public int Id { get; set; }
        public string PlatformName { get; set; } = null!;
        /// <summary>
        /// 歌曲ID
        /// </summary>
        public string MusicId { get; set; } = null!;
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string MusicName { get; set; } = null!;
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string MusicArtist { get; set; } = null!;
        public string MusicAlbum { get; set; } = null!;
    }
}
