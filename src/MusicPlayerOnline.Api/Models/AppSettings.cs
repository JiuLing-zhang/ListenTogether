namespace MusicPlayerOnline.Api.Models
{
    public class AppSettings
    {
        public string Secret { get; set; } = null!;
        public int RefreshTokenTTL { get; set; }
        public int TokenExpireDay { get; set; }
    }
}
