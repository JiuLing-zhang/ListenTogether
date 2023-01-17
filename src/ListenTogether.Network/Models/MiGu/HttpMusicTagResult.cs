namespace ListenTogether.Network.Models.MiGu;

public class HttpMusicTagResult
{
    public string title { get; set; }
    public string linkUrl { get; set; }
    public string singer { get; set; }
    public string album { get; set; }
    public string imgUrl { get; set; }
    public string ImageUrl => $"https:{imgUrl}";
}
