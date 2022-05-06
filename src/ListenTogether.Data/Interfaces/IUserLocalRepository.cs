using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;
public interface IUserLocalRepository
{
    public User? Read();
    public bool Write(User user);
    public void UpdateToken(TokenInfo? tokenInfo);
    public void Remove();
}