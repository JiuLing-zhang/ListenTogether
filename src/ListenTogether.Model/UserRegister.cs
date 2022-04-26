namespace ListenTogether.Model;
public class UserRegister
{
    public string Username { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string Password { get; set; } = null!;

    public UserAvatar Avatar { get; set; } = null!;
}

public class UserAvatar
{
    public byte[] File { get; set; } = null!;
    public string FileName { get; set; } = null!;
}