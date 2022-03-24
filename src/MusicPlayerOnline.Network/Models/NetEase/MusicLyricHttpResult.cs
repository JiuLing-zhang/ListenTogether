namespace MusicPlayerOnline.Network.Models.NetEase
{
    public class MusicLyricHttpResult
    {
        public bool sgc { get; set; }
        public bool sfy { get; set; }
        public bool qfy { get; set; }
        public Lyricuser lyricUser { get; set; }
        public Lrc lrc { get; set; }
        public Tlyric tlyric { get; set; }
        public int code { get; set; }
    }

    public class Lyricuser
    {
        public int id { get; set; }
        public int status { get; set; }
        public int demand { get; set; }
        public int userid { get; set; }
        public string nickname { get; set; }
        public long uptime { get; set; }
    }

    public class Lrc
    {
        public int version { get; set; }
        public string lyric { get; set; }
    }

    public class Tlyric
    {
        public int version { get; set; }
        public string lyric { get; set; }
    }

}
