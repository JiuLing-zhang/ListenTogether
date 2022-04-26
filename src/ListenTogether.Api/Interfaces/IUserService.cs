using ListenTogether.Api.Entities;
using ListenTogether.Api.Models;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Api.Interfaces;
public interface IUserService
{
    public Task<Result> Register(UserRegisterRequest registerUser, string avatarUrl);
    public Task<Result<UserResponse>> Login(UserRequest user, string deviceId);
    public Task Logout(int userId, string deviceId);
    public Task<Result<UserResponse>> RefreshToken(AuthenticateRequest authenticateInfo, string deviceId);
    public Task<Result<UserResponse>> GetUserInfo(int id);
    public Task<UserEntity?> GetOneEnableAsync(int id);
}