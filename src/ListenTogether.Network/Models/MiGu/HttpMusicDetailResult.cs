namespace ListenTogether.Network.Models.MiGu
{
    public class HttpMusicDetailResult
    {
        public Resource[] resource { get; set; } = null!;
    }

    public class Resource
    {
        public string contentId { get; set; } = null!;
        public string album { get; set; } = null!;
        public List<Albumimg> albumImgs { get; set; } = null!;
        public List<NewRateFormats> newRateFormats { get; set; } = null!;
        public string lrcUrl { get; set; } = null!;
    }

    public class Albumimg
    {
        public string imgSizeType { get; set; } = null!;
        public string img { get; set; } = null!;
    }

    public class NewRateFormats
    {
        public string formatType { get; set; } = null!;
        public string? url { get; set; } = null!;
        public string size { get; set; } = null!;
        public int SizeInt => JiuLing.CommonLibs.Text.RegexUtils.IsMatch(size, "^[0-9]*$") ? Convert.ToInt32(size) : 0;


        public string? iosUrl { get; set; } = null!;
        public string iosSize { get; set; } = null!;
        public int IosSizeInt => JiuLing.CommonLibs.Text.RegexUtils.IsMatch(iosSize, "^[0-9]*$") ? Convert.ToInt32(iosSize) : 0;


        public string androidUrl { get; set; } = null!;
        public string androidSize { get; set; } = null!;
        public int AndroidSizeInt => JiuLing.CommonLibs.Text.RegexUtils.IsMatch(androidSize, "^[0-9]*$") ? Convert.ToInt32(androidSize) : 0;

    }
}
