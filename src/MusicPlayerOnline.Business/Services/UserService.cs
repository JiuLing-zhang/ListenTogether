using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class UserService : IUserService
{
    public Task<bool> Register(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<User> Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Logout()
    {
        throw new NotImplementedException();
    }
}