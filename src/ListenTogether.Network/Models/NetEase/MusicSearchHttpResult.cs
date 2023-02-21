namespace ListenTogether.Network.Models.NetEase
{
    public class MusicSearchHttpResult
    {
        public List<MusicSearchHttpResultSong> songs { get; set; } = null!;
    }

    public class MusicSearchHttpResultSong
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        /// <summary>
        /// 艺人信息
        /// </summary>
        public List<MusicSearchHttpResultSongArtist> ar { get; set; } = null!;
        /// <summary>
        /// 专辑
        /// </summary>
        public MusicSearchHttpResultSongAlbum al { get; set; } = null!;
        /// <summary>
        /// 时长
        /// </summary>
        public int dt { get; set; }

        public MusicSearchHttpResultSongPrivilege privilege { get; set; } = null!;
    }

    public class MusicSearchHttpResultSongAlbum
    {
        public string name { get; set; } = null!;
        public string picUrl { get; set; } = null!;
    }

    public class MusicSearchHttpResultSongArtist
    {
        public string name { get; set; } = null!;
    }

    public class MusicSearchHttpResultSongPrivilege
    {
        /// <summary>
        /// 费用类型
        /// </summary>
        public int fee { get; set; }
    }
}
