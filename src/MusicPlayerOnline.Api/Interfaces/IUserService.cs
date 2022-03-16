using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Model.Request;
using MusicPlayerOnline.Model.Response;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IUserService
    {
        public Task<JsonResultDto> Register(User dto, string ipAddress);
        public Task<UserDto> Login(User dto, string ipAddress);

        public Task<UserEntity?> GetOneEnableAsync(int id);
    }
}
