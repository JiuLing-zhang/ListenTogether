namespace ListenTogether.Network.Models.KuWo
{
    public class MusicDetailHttpResult
    {
        public Lrclist[] lrclist { get; set; } = null!;
        public Songinfo songinfo { get; set; } = null!;
    }

    public class Songinfo
    {
        public string pic { get; set; } = null!;
    }

    public class Lrclist
    {
        public string lineLyric { get; set; } = null!;
        public string time { get; set; } = null!;
    }
}
