using ListenTogether.Data.Interfaces;
using ListenTogether.EasyLog;
using ListenTogether.Model;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Data.Repositories.Api;
public class UserApiRepository : IUserApiRepository
{
    public async Task<(bool Succeed, string Message)> RegisterAsync(UserRegister registerUser)
    {
        try
        {
            var mfdc = new MultipartFormDataContent();
            mfdc.Add(new StringContent(registerUser.Username), nameof(registerUser.Username));
            mfdc.Add(new StringContent(registerUser.Nickname), nameof(registerUser.Nickname));
            mfdc.Add(new StringContent(registerUser.Password), nameof(registerUser.Password));
            //头像
            mfdc.Add(new StreamContent(new MemoryStream(registerUser.Avatar.File)), "Avatar", registerUser.Avatar.FileName);

            var response = await DataConfig.HttpClientWithNoToken.PostAsync(DataConfig.ApiSetting.User.Register, mfdc);
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
            var response = await DataConfig.HttpClientWithNoToken.PostAsync(DataConfig.ApiSetting.User.Login, sc);
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
            var response = await DataConfig.HttpClientWithNoToken.PostAsync(DataConfig.ApiSetting.User.Logout, null);
            await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Logger.Error("账号退出失败。", ex);
        }
    }
}