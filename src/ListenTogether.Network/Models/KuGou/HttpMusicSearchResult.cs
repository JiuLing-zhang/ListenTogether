namespace ListenTogether.Network.Models.KuGou
{
    public class HttpMusicSearchResult
    {
        public List[] lists { get; set; }
    }

    public class List
    {
        public string SongName { get; set; }

        public string FileHash { get; set; }

        public string AlbumID { get; set; }
        public string AlbumName { get; set; }

        public string ID { get; set; }

        public int Duration { get; set; }

        public string SingerName { get; set; }
        public int Privilege { get; set; }
    }
}
