namespace ListenTogether.Network.Models.MiGu
{
    public class HttpMusicDetailResult
    {
        public List<HttpMusicDetailResource>? resource { get; set; }
    }

    public class HttpMusicDetailResource
    {
        public List<HttpMusicDetailResourceNewRateFormats> newRateFormats { get; set; } = null!;
        public string lrcUrl { get; set; } = null!;
    }

    public class HttpMusicDetailResourceNewRateFormats
    {
        public string formatType { get; set; } = null!;
        public string? url { get; set; } = null!;
        public string? iosUrl { get; set; } = null!;
    }
}
