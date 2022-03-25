using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces;
public interface IUserService
{
    public Task<Result> Register(User user);
    public Task<Result<UserDto>> Login(User user, string deviceId);
    public Task Logout(int userId, string deviceId);
    public Task<Result<UserDto>> RefreshToken(AuthenticateInfo authenticateInfo, string deviceId);
    public Task<Result<UserDto>> GetUserInfo(int id);
    public Task<UserEntity?> GetOneEnableAsync(int id);
}