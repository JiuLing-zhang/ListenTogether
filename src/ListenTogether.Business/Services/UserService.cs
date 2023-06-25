using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;

public class UserService : IUserService
{
    private readonly IUserApiRepository _repository;
    public UserService(IUserApiRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Succeed, string Message)> RegisterAsync(UserRegister registerUser)
    {
        return await _repository.RegisterAsync(registerUser);
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        return await _repository.LoginAsync(username, password);
    }

    public async Task LogoutAsync()
    {
        await _repository.LogoutAsync();
    }

    public async Task<(bool Succeed, string Message)> EditUserInfoAsync(UserEdit user)
    {
        return await _repository.EditUserInfoAsync(user);
    }
}