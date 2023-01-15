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
        public string id { get; set; }
        public string name { get; set; }
        public string mediumPic { get; set; }
        public List<HttpMusicTopSongSingerResult>? singers { get; set; }
        public HttpMusicTopSongAlbumResult? album { get; set; }
    }

    public class HttpMusicTopSongAlbumResult
    {
        public string albumId { get; set; }
        public string albumName { get; set; }
        public int isDigitalAlbum { get; set; }
    }
    public class HttpMusicTopSongSingerResult
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
