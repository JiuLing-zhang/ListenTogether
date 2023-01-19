namespace ListenTogether.Network.Models.NetEase
{
    public class TagMusicHttpResult
    {
        public int code { get; set; }
        public TagMusicPlaylistHttpResult? playlist { get; set; }
    }

    public class TagMusicPlaylistHttpResult
    {
        public List<TagMusicTrack>? tracks { get; set; }
    }

    public class TagMusicTrack
    {
        public string name { get; set; }
        public int id { get; set; }
        public List<TagMusicAr>? ar { get; set; }
        public TagMusicAl? al { get; set; }
        public long dt { get; set; }
    }

    public class TagMusicAl
    {
        public string name { get; set; }
        public string picUrl { get; set; }

    }
    public class TagMusicAr
    {
        public string name { get; set; }
    }

}
