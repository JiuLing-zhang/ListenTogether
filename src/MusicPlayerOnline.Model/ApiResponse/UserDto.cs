namespace MusicPlayerOnline.Model.Response
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string JwtToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
