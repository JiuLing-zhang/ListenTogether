namespace MusicPlayerOnline.Network.Models.NetEase
{
    public class MusicSearchHttpResult
    {
        public Song[] songs { get; set; }
        public int songCount { get; set; }
    }

    public class Song
    {
        public int id { get; set; }
        public string name { get; set; }
        public string[] alia { get; set; }
        /// <summary>
        /// 艺人信息
        /// </summary>
        public Artist[] ar { get; set; }
        /// <summary>
        /// 专辑
        /// </summary>
        public Album al { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int dt { get; set; }
        /// <summary>
        /// 费用类型
        /// </summary>
        public int fee { get; set; }
    }

    public class Album
    {
        public string name { get; set; }
        public string picUrl { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
    }
}
