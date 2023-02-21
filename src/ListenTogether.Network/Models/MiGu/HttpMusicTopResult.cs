namespace ListenTogether.Network.Models.MiGu
{
    public class HttpMusicTopResult
    {
        public HttpMusicTopSongResult? songs { get; set; }
    }

    public class HttpMusicTopSongResult
    {
        public List<HttpMusicTopSongItemResult>? items { get; set; }
    }

    public class HttpMusicTopSongItemResult
    {
        public string copyrightId { get; set; } = null!;
        public string name { get; set; } = null!;
        public string mediumPic { get; set; } = null!;
        public string ImageUrl => $"https:{mediumPic}";
        public string duration { get; set; } = null!;
        public List<HttpMusicTopSongSingerResult>? singers { get; set; }
        public HttpMusicTopSongAlbumResult? album { get; set; }
    }

    public class HttpMusicTopSongAlbumResult
    {
        public string albumName { get; set; } = null!;
    }
    public class HttpMusicTopSongSingerResult
    {
        public string name { get; set; } = null!;
    }

}
