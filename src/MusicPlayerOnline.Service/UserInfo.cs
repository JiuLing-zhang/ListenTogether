namespace MusicPlayerOnline.Service
{
    public class UserInfo
    {
        public UserInfo(string userName, string nickname, string avatar, string jwtToken, string refreshToken)
        {
            UserName = userName;
            Nickname = nickname;
            Avatar = avatar;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
        public string UserName { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
