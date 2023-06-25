using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;

public interface IUserApiRepository
{
    public Task<(bool Succeed, string Message)> RegisterAsync(UserRegister registerUser);
    public Task<User?> LoginAsync(string username, string password);
    public Task LogoutAsync();
    public Task<(bool Succeed, string Message)> EditUserInfoAsync(UserEdit user);
}