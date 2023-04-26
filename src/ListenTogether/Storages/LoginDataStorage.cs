using ListenTogether.Data;

namespace ListenTogether.Storages;
public class LoginDataStorage : ILoginDataStorage
{
    public string GetUsername()
    {
        return Preferences.Get("Username", "");
    }
    public void SetUsername(string value)
    {
        Preferences.Set("Username", value);
    }

    public string GetNickname()
    {
        return Preferences.Get("Nickname", "");
    }
    public void SetNickname(string value)
    {
        Preferences.Set("Nickname", value);
    }

    public string GetAvatar()
    {
        return Preferences.Get("Avatar", "");
    }
    public void SetAvatar(string value)
    {
        Preferences.Set("Avatar", value);
    }

    public string GetToken()
    {
        return Preferences.Get("Token", "");
    }
    public void SetToken(string value)
    {
        Preferences.Set("Token", value);
    }
    public string GetRefreshToken()
    {
        return Preferences.Get("RefreshToken", "");
    }
    public void SetRefreshToken(string value)
    {
        Preferences.Set("RefreshToken", value);
    }

    public void Clear()
    {
        SetUsername("");
        SetNickname("");
        SetAvatar("");
        SetToken("");
        SetRefreshToken("");
    }
}