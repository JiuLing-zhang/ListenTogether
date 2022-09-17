namespace ListenTogether.Network.Models.KuWo
{
    public class HttpMusicSearchResult
    {
        public HttpMusicList[] list { get; set; } = null!;
    }

    public class HttpMusicList
    {
        public string artist { get; set; } = null!;
        public int rid { get; set; }
        public int duration { get; set; }
        public string album { get; set; } = null!;
        public string name { get; set; } = null!;
        public Payinfo payInfo { get; set; } = null!;
    }

    public class Payinfo
    {
        public string listen_fragment { get; set; } = null!;
    }

}
