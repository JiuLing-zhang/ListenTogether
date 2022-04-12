using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
//TODO 文件重命名
public interface IUserLocalRepository
{
    public User? Read();
    public bool Write(User user);
    public void UpdateToken(TokenInfo tokenInfo);
    public void Remove();
}