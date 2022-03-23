using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Service.Interfaces;
public interface IUserService
{
    public Task<Result> Register(string username, string password);
    public Task<Result<UserDto>> Login(string username, string password);
    public Task<Result> Logout();
}