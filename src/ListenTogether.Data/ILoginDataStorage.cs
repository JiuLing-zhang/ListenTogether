namespace ListenTogether.Data;
public interface ILoginDataStorage
{
    string GetUsername();
    void SetUsername(string value);
    string GetNickname();
    void SetNickname(string value);
    string GetAvatar();
    void SetAvatar(string value);
    string GetToken();
    void SetToken(string value);
    string GetRefreshToken();
    void SetRefreshToken(string value);
    void Clear();
}