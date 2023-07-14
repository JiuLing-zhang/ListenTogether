using ListenTogether.Data.Interface;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Data.Api.Repositories;
public class UserApiRepository : IUserApiRepository
{
    private readonly IHttpClientFactory _httpClientFactory = null!;
    public UserApiRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<(bool Succeed, string Message)> RegisterAsync(UserRegister registerUser)
    {
        try
        {
            var sc = new StringContent(registerUser.ToJson(), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClientFactory.CreateClient("WebAPINoToken").PostAsync(DataConfig.ApiSetting.User.Register, sc);
            var json = await response.Content.ReadAsStringAsync();
            var result = json.ToObject<Result>();
            if (result == null || result.Code != 0)
            {
                return (false, result == null ? "" : result.Message);
            }
            return (true, result.Message);
        }
        catch (Exception ex)
        {
            Logger.Error("注册失败。", ex);
            return (false, "注册失败，系统错误");
        }
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        try
        {
            var data = new UserRequest()
            {
                Username = username,
                Password = password
            };
            var sc = new StringContent(data.ToJson(), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClientFactory.CreateClient("WebAPINoToken").PostAsync(DataConfig.ApiSetting.User.Login, sc);
            var json = await response.Content.ReadAsStringAsync();

            var result = json.ToObject<Result<UserResponse>>();
            if (result == null || result.Data == null)
            {
                return default;
            }

            return new User()
            {
                Username = result.Data.Username,
                Nickname = result.Data.Nickname,
                Avatar = result.Data.Avatar,
                Token = result.Data.Token,
                RefreshToken = result.Data.RefreshToken,
            };
        }
        catch (Exception ex)
        {
            Logger.Error("登录失败。", ex);
            return default;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            var response = await _httpClientFactory.CreateClient("WebAPINoToken").PostAsync(DataConfig.ApiSetting.User.Logout, null);
            await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Logger.Error("账号退出失败。", ex);
        }
    }

    public async Task<(User? UserResult, string ErrorMessage)> EditUserInfoAsync(UserEdit user)
    {
        try
        {
            var mfdc = new MultipartFormDataContent();
            if (user.Username.IsNotEmpty())
            {
                mfdc.Add(new StringContent(user.Username), nameof(user.Username));
            }
            if (user.Nickname.IsNotEmpty())
            {
                mfdc.Add(new StringContent(user.Nickname), nameof(user.Nickname));
            }
            if (user.Avatar != null)
            {
                mfdc.Add(new StreamContent(new MemoryStream(user.Avatar.File)), "Avatar", user.Avatar.FileName);
            }

            var response = await _httpClientFactory.CreateClient("WebAPI").PostAsync(DataConfig.ApiSetting.User.Edit, mfdc);
            var json = await response.Content.ReadAsStringAsync();
            var result = json.ToObject<Result<UserResponse>>();
            if (result == null || result.Code != 0)
            {
                return (default, result?.Message ?? "网络错误");
            }
            return (new User()
            {
                Username = result.Data.Username,
                Nickname = result.Data.Nickname,
                Avatar = result.Data.Avatar
            }, "");
        }
        catch (Exception ex)
        {
            Logger.Error("编辑用户信息失败。", ex);
            return (default, "网络连接失败");
        }
    }
}