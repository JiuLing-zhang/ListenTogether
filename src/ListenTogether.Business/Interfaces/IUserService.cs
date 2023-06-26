using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;
public interface IUserService
{
    public Task<(bool Succeed, string Message)> RegisterAsync(UserRegister registerUser);
    public Task<User?> LoginAsync(string username, string password);
    public Task LogoutAsync();
    public Task<(User? UserResult, string ErrorMessage)> EditUserInfoAsync(UserEdit user);
}