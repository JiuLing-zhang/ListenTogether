using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.Model
{
    public class Playlist
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
