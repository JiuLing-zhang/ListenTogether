using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Service.Interfaces;
using MusicPlayerOnline.Service.Net;

namespace MusicPlayerOnline.Service.Services;
public class UserConfigApiService : IUserConfigService
{
    private readonly IHttpClientProvider _httpClient;
    public UserConfigApiService(IHttpClientProvider httpClientProvider)
    {
        _httpClient = httpClientProvider;
    }

    public async Task<UserSettingDto?> ReadAllSettingsAsync()
    {
        var json = await _httpClient.GetStringWithTokenAsync(GlobalConfig.ApiSetting.UserConfig.Get);
        return JsonSerializer.Deserialize<UserSettingDto>(json);
    }

    public async Task<Result> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        var json = await _httpClient.PostReadAsStringWithTokenAsync(GlobalConfig.ApiSetting.UserConfig.WriteGeneral, generalSetting);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }

    public async Task<Result> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        var json = await _httpClient.PostReadAsStringWithTokenAsync(GlobalConfig.ApiSetting.UserConfig.WriteSearchConfig, searchSetting);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }

    public async Task<Result> WritePlaySettingAsync(PlaySetting playSetting)
    {
        var json = await _httpClient.PostReadAsStringWithTokenAsync(GlobalConfig.ApiSetting.UserConfig.WritePlayConfig, playSetting);
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null)
        {
            return new Result(999, "连接服务器失败");
        }
        return obj;
    }
}