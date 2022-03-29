using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface IUserRepository
{
    public Task<bool> Register(string username, string password);
    public Task<User?> Login(string username, string password);
    public Task<bool> Logout();
}