namespace ListenTogether.Network.Models.MiGu
{
    public class HttpMusicSearchResult
    {
        public string id => linkUrl.Replace("/v3/music/song/", "");
        public string type { get; set; } = null!;
        public string title { get; set; } = null!;
        public string singer { get; set; } = null!;
        public string album { get; set; } = null!;
        public string linkUrl { get; set; } = null!;
        public string imgUrl { get; set; } = null!;
    }
}
