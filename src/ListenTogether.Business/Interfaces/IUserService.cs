using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;
public interface IUserService
{
    public Task<(bool Succeed, string Message)> Register(UserRegister registerUser);
    public Task<User?> Login(string username, string password);
    public Task<bool> Logout();
}