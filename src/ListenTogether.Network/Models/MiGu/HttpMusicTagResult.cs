namespace ListenTogether.Network.Models.MiGu;

public class HttpMusicTagResult
{
    public string title { get; set; } = null!;
    public string linkUrl { get; set; } = null!;
    public string singer { get; set; } = null!;
    public string album { get; set; } = null!;
    public string imgUrl { get; set; } = null!;
    public string ImageUrl => $"https:{imgUrl}";
}
