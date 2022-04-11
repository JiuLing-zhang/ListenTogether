using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
//TODO 文件重命名 IUserApiRepository
public interface IUserRepository
{
    public Task<(bool Succeed, string Message)> Register(UserRegister registerUser);
    public Task<User?> Login(string username, string password);
    public Task<bool> Logout();
}