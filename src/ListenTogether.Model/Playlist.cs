using ListenTogether.Model.Enums;

namespace ListenTogether.Model
{
    public class Playlist
    {
        /// <summary>
        /// 歌曲ID
        /// </summary>
        public string MusicId { get; set; } = null!;

        /// <summary>
        /// 平台
        /// </summary>
        public PlatformEnum Platform { get; set; }
        /// <summary>
        /// 歌曲在平台的ID
        /// </summary>
        public string MusicIdOnPlatform { get; set; } = null!;

        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string MusicName { get; set; } = null!;
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string MusicArtist { get; set; } = null!;
        public string MusicAlbum { get; set; } = null!;

        public string MusicImageUrl { get; set; } = null!;
        public DateTime EditTime { get; set; }
    }
}
