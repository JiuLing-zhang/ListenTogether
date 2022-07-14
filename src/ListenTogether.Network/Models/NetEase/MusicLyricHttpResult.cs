namespace ListenTogether.Network.Models.NetEase
{
    public class MusicLyricHttpResult
    {
        public Lrc lrc { get; set; }
        public int code { get; set; }
    }

    public class Lrc
    {
        public string lyric { get; set; }
    }
}
