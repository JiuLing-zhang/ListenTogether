using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IUserService
    {
        public Task<Result> Register(User dto, string ipAddress);
        public Task<Result<UserDto>> Login(User dto, string ipAddress);
        public Task<UserEntity?> GetOneEnableAsync(int id);
    }
}
