using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces;
public interface IUserService
{
    public Task<Result> Register(User dto, string ipAddress);
    public Task<Result<UserDto>> Login(User dto, string ipAddress);
    public Task<Result> Logout(int id);
    public Task<Result<UserDto>> GetUserInfo(int id);
    public Task<UserEntity?> GetOneEnableAsync(int id);
}