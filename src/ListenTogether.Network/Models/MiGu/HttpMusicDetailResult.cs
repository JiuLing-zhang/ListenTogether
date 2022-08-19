﻿namespace ListenTogether.Network.Models.MiGu
{
    public class HttpMusicDetailResult
    {
        public Resource[] resource { get; set; }
    }

    public class Resource
    {
        public string contentId { get; set; }
        public string album { get; set; }
        public List<Albumimg> albumImgs { get; set; }
        public List<NewRateFormats> newRateFormats { get; set; }
        public string lrcUrl { get; set; }
    }

    public class Albumimg
    {
        public string imgSizeType { get; set; }
        public string img { get; set; }
    }

    public class NewRateFormats
    {
        public string url { get; set; }
        public string size { get; set; }

        public int SizeInt => JiuLing.CommonLibs.Text.RegexUtils.IsMatch(size, "^[0-9]*$") ? Convert.ToInt32(size) : 0;
        public string fileType { get; set; }
    }
}
