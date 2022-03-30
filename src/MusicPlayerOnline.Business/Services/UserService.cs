using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Register(string username, string password)
    {
        return await _repository.Register(username, password);
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