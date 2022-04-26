using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;

public interface IUserApiRepository
{
    public Task<(bool Succeed, string Message)> Register(UserRegister registerUser);
    public Task<User?> Login(string username, string password);
    public Task<bool> Logout();
}