using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;
public interface IUserService
{
    public Task<bool> Register(string username, string password);
    public Task<User> Login(string username, string password);
    public Task<bool> Logout();
}