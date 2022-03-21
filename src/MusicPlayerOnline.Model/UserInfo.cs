namespace MusicPlayerOnline.Model;
public class UserInfo
{
    public UserInfo(string userName, string nickname, string avatar, string token, string refreshToken)
    {
        UserName = userName;
        Nickname = nickname;
        Avatar = avatar;
        Token = token;
        RefreshToken = refreshToken;
    }
    public string UserName { get; set; }
    public string Nickname { get; set; }
    public string Avatar { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
