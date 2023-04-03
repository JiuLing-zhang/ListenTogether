namespace ListenTogether.Network.Models.KuWo
{
    public class HttpMusicSearchResult
    {
        public List<HttpMusicSearchResultList> list { get; set; } = null!;
    }

    public class HttpMusicSearchResultList
    {
        public string artist { get; set; } = null!;
        public int rid { get; set; }
        public int duration { get; set; }
        public string album { get; set; } = null!;
        public string name { get; set; } = null!;
        public string pic { get; set; } = null!;
        public HttpMusicSearchResultListPayinfo payInfo { get; set; } = null!;
    }

    public class HttpMusicSearchResultListPayinfo
    {
        public string listen_fragment { get; set; } = null!;
    }

}
