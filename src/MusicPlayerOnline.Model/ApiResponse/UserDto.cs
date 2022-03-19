namespace MusicPlayerOnline.Model.ApiResponse
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
