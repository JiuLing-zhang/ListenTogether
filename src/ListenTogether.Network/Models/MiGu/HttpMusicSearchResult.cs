namespace ListenTogether.Network.Models.MiGu
{
    public class HttpMusicSearchResult
    {
        public string id => linkUrl.Replace("/v3/music/song/", "");
        public string type { get; set; }
        public string title { get; set; }
        public string singer { get; set; }
        public string album { get; set; }
        public string linkUrl { get; set; }
        public string imgUrl { get; set; }
    }
}
