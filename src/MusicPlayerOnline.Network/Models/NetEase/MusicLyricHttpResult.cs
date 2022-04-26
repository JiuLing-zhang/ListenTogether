namespace MusicPlayerOnline.Network.Models.NetEase
{
    public class MusicLyricHttpResult
    {
        public Lrc lrc { get; set; }
        public int code { get; set; }
    }

    public class Lrc
    {
        public int version { get; set; }
        public string lyric { get; set; }
    }
}
