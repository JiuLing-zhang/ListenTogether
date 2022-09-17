namespace ListenTogether.Network.Models.NetEase
{
    public class MusicSearchHttpResult
    {
        public Song[] songs { get; set; } = null!;
        public int songCount { get; set; }
    }

    public class Song
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public string[] alia { get; set; } = null!;
        /// <summary>
        /// 艺人信息
        /// </summary>
        public Artist[] ar { get; set; } = null!;
        /// <summary>
        /// 专辑
        /// </summary>
        public Album al { get; set; } = null!;
        /// <summary>
        /// 时长
        /// </summary>
        public int dt { get; set; }

        public Privilege privilege { get; set; } = null!;
    }

    public class Album
    {
        public string name { get; set; } = null!;
        public string picUrl { get; set; } = null!;
    }

    public class Artist
    {
        public string name { get; set; } = null!;
    }

    public class Privilege
    {
        /// <summary>
        /// 费用类型
        /// </summary>
        public int fee { get; set; }
    }
}
