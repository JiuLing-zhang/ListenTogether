using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;

namespace MusicPlayerOnline.Api.Interfaces;
public interface IUserService
{
    public Task<Result> Register(UserRequest user);
    public Task<Result<UserResponse>> Login(UserRequest user, string deviceId);
    public Task Logout(int userId, string deviceId);
    public Task<Result<UserResponse>> RefreshToken(AuthenticateRequest authenticateInfo, string deviceId);
    public Task<Result<UserResponse>> GetUserInfo(int id);
    public Task<UserEntity?> GetOneEnableAsync(int id);
}