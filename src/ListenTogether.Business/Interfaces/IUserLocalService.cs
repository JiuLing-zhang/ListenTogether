using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;

public interface IUserLocalService
{
    public User? Read();
    public bool Write(User user);
    public void UpdateToken(TokenInfo tokenInfo);
    public void Remove();
}