namespace ListenTogether.Storage;
public class UserInfoStorage
{
    public static string GetUsername()
    {
        return Preferences.Get("Username", "");
    }
    public static void SetUsername(string value)
    {
        Preferences.Set("Username", value);
    }

    public static string GetNickname()
    {
        return Preferences.Get("Nickname", "");
    }
    public static void SetNickname(string value)
    {
        Preferences.Set("Nickname", value);
    }

    public static string GetAvatar()
    {
        return Preferences.Get("Avatar", "");
    }
    public static void SetAvatar(string value)
    {
        Preferences.Set("Avatar", value);
    }

    public static string GetToken()
    {
        return Preferences.Get("Token", "");
    }
    public static void SetToken(string value)
    {
        Preferences.Set("Token", value);
    }
    public static string GetRefreshToken()
    {
        return Preferences.Get("RefreshToken", "");
    }
    public static void SetRefreshToken(string value)
    {
        Preferences.Set("RefreshToken", value);
    }

    public static void Clear()
    {
        SetUsername("");
        SetNickname("");
        SetAvatar("");
        SetToken("");
        SetRefreshToken("");
    }
}