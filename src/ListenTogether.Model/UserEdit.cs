namespace ListenTogether.Model;
public class UserEdit
{
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public UserAvatar? Avatar { get; set; }
}

public class UserAvatar
{
    public byte[] File { get; set; } = null!;
    public string FileName { get; set; } = null!;
}