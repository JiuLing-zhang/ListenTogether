using ListenTogether.Api.Entities;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Api.Interfaces;
public interface IUserService
{
    public Task<Result> RegisterAsync(UserRegisterRequest registerUser);
    public Task<Result<UserResponse>> LoginAsync(UserRequest user, string deviceId);
    public Task LogoutAsync(int userId, string deviceId);
    public Task<Result<UserResponse>> RefreshTokenAsync(AuthenticateRequest authenticateInfo, string deviceId);
    public Task<Result<UserResponse>> GetUserInfoAsync(int id);
    public Task<UserEntity?> GetOneEnableAsync(int id);
    public Task<Result> EditUserInfoAsync(int id, UserEditRequest user);
}