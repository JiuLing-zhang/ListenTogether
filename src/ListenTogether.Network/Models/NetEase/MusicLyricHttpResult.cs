namespace ListenTogether.Network.Models.NetEase
{
    public class MusicLyricHttpResult
    {
        public MusicLyricHttpResultLrc? lrc { get; set; }
        public int code { get; set; }
    }

    public class MusicLyricHttpResultLrc
    {
        public string lyric { get; set; } = null!;
    }
}
