namespace ListenTogether.Model.Api.Response
{
    public class UserResponse
    {
        public string Username { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
