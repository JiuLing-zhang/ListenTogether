using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model
{
    public class MyFavoriteDetail
    {
        public int Id { get; set; }
        public int MyFavoriteId { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public int Platform { get; set; }
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
