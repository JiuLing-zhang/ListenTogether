using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;

namespace MusicPlayerOnline.Service.Services;
public class UserService : IUserService
{
    private readonly IHttpClientProvider _httpClient;
    public UserService(IHttpClientProvider httpClientProvider)
    {
        _httpClient = httpClientProvider;
    }

    public async Task<Result> Register(string username, string password)
    {
        var data = new User()
        {
            Username = username,
            Password = password
        };
        var sc = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
        var json = await _httpClient.PostReadAsStringWithNoTokenAsync(GlobalConfig.ApiSetting.User.Register, sc);

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
        var json = await _httpClient.PostReadAsStringWithNoTokenAsync(GlobalConfig.ApiSetting.User.Login, sc);

        var result = JsonSerializer.Deserialize<Result<UserDto>>(json);
        if (result == null || result.Data == null)
        {
            return new Result<UserDto>(999, "连接服务器失败", null);
        }

        GlobalConfig.CurrentUser = new UserInfo(result.Data.UserName, result.Data.Nickname, result.Data.Avatar, result.Data.Token, result.Data.RefreshToken);
        return result;
    }

    public async Task<Result> Logout()
    {
        var json = await _httpClient.PostReadAsStringWithNoTokenAsync(GlobalConfig.ApiSetting.User.Logout);
        var result = JsonSerializer.Deserialize<Result>(json);
        if (result == null)
        {
            return new Result(999, "连接服务器失败");
        }

        GlobalConfig.CurrentUser = null;
        return result;
    }
}