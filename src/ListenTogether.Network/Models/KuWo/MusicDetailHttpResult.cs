namespace ListenTogether.Network.Models.KuWo
{
    public class MusicDetailHttpResult
    {
        public List<MusicDetailLrclist>? lrclist { get; set; }
    }
    public class MusicDetailLrclist
    {
        public string lineLyric { get; set; } = null!;
        public string time { get; set; } = null!;
    }
}
