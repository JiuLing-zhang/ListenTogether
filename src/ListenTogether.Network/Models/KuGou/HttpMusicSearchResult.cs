namespace ListenTogether.Network.Models.KuGou
{
    public class HttpMusicSearchResult
    {
        public List[] lists { get; set; } = null!;
    }

    public class List
    {
        public string SongName { get; set; } = null!;

        public string FileHash { get; set; } = null!;

        public string AlbumID { get; set; } = null!;
        public string AlbumName { get; set; } = null!;

        public string ID { get; set; } = null!;

        public int Duration { get; set; }

        public string SingerName { get; set; } = null!;
        public int Privilege { get; set; }
    }
}
