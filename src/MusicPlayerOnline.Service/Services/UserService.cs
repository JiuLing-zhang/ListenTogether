using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient = new();

        public async Task<Result> Register(string username, string password)
        {
            var data = new User()
            {
                Username = username,
                Password = password
            };
            var sc = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(GlobalConfig.ApiSetting.User.Register, sc);
            string json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result>(json);
            if (result == null)
            {
                return new Result(999, "连接服务器失败");
            }
            return result;
        }

        public async Task<Result<UserDto>> Login(string username, string password)
        {
            var data = new User()
            {
                Username = username,
                Password = password
            };
            var sc = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(GlobalConfig.ApiSetting.User.Login, sc);
            string json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Result<UserDto>>(json);
            if (result == null)
            {
                return new Result<UserDto>(999, "连接服务器失败", null);
            }
            return result;
        }
    }
}
