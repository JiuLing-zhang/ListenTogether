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

    public async Task<(bool Succeed, string Message)> Register(UserRegister registerUser)
    {
        return await _repository.Register(registerUser);
    }

    public async Task<User?> Login(string username, string password)
    {
        return await _repository.Login(username, password);
    }

    public async Task<bool> Logout()
    {
        return await _repository.Logout();
    }
}