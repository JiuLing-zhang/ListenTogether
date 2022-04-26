namespace MusicPlayerOnline.Network.Models.MiGu
{
    public class HttpMusicDetailResult
    {
        public string code { get; set; }
        public string info { get; set; }
        public Resource[] resource { get; set; }
    }

    public class Resource
    {
        public string contentId { get; set; }
        public string album { get; set; }
        public List<Albumimg> albumImgs { get; set; }
        public string lrcUrl { get; set; }
    }

    public class Albumimg
    {
        public string imgSizeType { get; set; }
        public string img { get; set; }
    }
}
