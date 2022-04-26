using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;

public interface IUserLocalService
{
    public User? Read();
    public bool Write(User user);
    public void UpdateToken(TokenInfo tokenInfo);
    public void Remove();
}