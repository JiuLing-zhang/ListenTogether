namespace ListenTogether.Network.Models.KuWo
{
    public class MusicDetailHttpResult
    {
        public Lrclist[] lrclist { get; set; }
        public Songinfo songinfo { get; set; }
    }

    public class Songinfo
    {
        public string pic { get; set; }
    }

    public class Lrclist
    {
        public string lineLyric { get; set; }
        public string time { get; set; }
    }










    public class Info
    {
        public int id { get; set; }
        public string name { get; set; }
        public string pic { get; set; }
        public int album_id { get; set; }
        public string album_name { get; set; }
        public string album_pic { get; set; }
        public int artist_id { get; set; }
        public string artist_name { get; set; }
        public string artist_alt { get; set; }
        public string artist_pic { get; set; }
        public int isstar { get; set; }
        public string content_type { get; set; }
        public int mv_status { get; set; }
        public string pay { get; set; }
        public int online { get; set; }
    }

    public class Lrc
    {
        public string lineLyric { get; set; }
        public string time { get; set; }
    }

}
