namespace ListenTogether.Network.Models.KuWo
{
    public class HttpMusicSearchResult
    {
        public HttpMusicList[] list { get; set; }
    }

    public class HttpMusicList
    {
        public string artist { get; set; }
        public int rid { get; set; }
        public string album { get; set; }
        public string name { get; set; }
        public Payinfo payInfo { get; set; }
    }

    public class Payinfo
    {
        public string listen_fragment { get; set; }
    }
 
}
