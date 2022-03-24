namespace MusicPlayerOnline.Network.Models.KuGou
{
    public class MusicLyricHttpResult
    {
        public string hash { get; set; }
        public int timelength { get; set; }
        public int filesize { get; set; }
        public string audio_name { get; set; }
        public int have_album { get; set; }
        public string album_name { get; set; }
        public string album_id { get; set; }
        public string img { get; set; }
        public int have_mv { get; set; }
        public int video_id { get; set; }
        public string author_name { get; set; }
        public string song_name { get; set; }
        public string lyrics { get; set; }
        public string author_id { get; set; }
        public int privilege { get; set; }
        public string privilege2 { get; set; }
        public string play_url { get; set; }
        public Author[] authors { get; set; }
        public int is_free_part { get; set; }
        public int bitrate { get; set; }
        public string recommend_album_id { get; set; }
        public string audio_id { get; set; }
        public bool has_privilege { get; set; }
        public string play_backup_url { get; set; }
    }

    public class Author
    {
        public string author_id { get; set; }
        public string author_name { get; set; }
        public string is_publish { get; set; }
        public string sizable_avatar { get; set; }
        public string avatar { get; set; }
    }

}
