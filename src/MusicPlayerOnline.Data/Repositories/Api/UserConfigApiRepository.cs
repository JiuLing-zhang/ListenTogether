using System.Text.Json;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Api;
//TODO 删除文件，配置保存在本地
public class UserConfigApiRepository
{
    public async Task<UserSetting> ReadAllSettingsAsync()
    {
        var json = await DataConfig.HttpClientWithToken.GetStringAsync(DataConfig.ApiSetting.UserConfig.Get);
        var userSettingDto = JsonSerializer.Deserialize<UserSettingResponse>(json);
        if (userSettingDto == null)
        {
            throw new Exception("获取用户配置失败");
        }

        return new UserSetting()
        {
            General = new GeneralSetting()
            {
                IsAutoCheckUpdate = userSettingDto.General.IsAutoCheckUpdate,
                IsHideWindowWhenMinimize = userSettingDto.General.IsHideWindowWhenMinimize,
            },
            Play = new PlaySetting()
            {
                IsAutoNextWhenFailed = userSettingDto.Play.IsAutoNextWhenFailed,
                IsCleanPlaylistWhenPlayMyFavorite = userSettingDto.Play.IsCleanPlaylistWhenPlayMyFavorite,
                IsWifiPlayOnly = userSettingDto.Play.IsWifiPlayOnly,
            },
            Search = new SearchSetting()
            {
                EnablePlatform = (PlatformEnum)userSettingDto.Search.EnablePlatform,
                IsCloseSearchPageWhenPlayFailed = userSettingDto.Search.IsCloseSearchPageWhenPlayFailed,
                IsHideShortMusic = userSettingDto.Search.IsHideShortMusic,
            }
        };
    }

    public async Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        var request = new GeneralSettingRequest()
        {
            IsAutoCheckUpdate = generalSetting.IsAutoCheckUpdate,
            IsHideWindowWhenMinimize = generalSetting.IsHideWindowWhenMinimize,
        };
        string content = JsonSerializer.Serialize(request);

        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.UserConfig.WriteGeneral, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        var request = new SearchSettingRequest()
        {
            EnablePlatform = (int)searchSetting.EnablePlatform,
            IsCloseSearchPageWhenPlayFailed = searchSetting.IsCloseSearchPageWhenPlayFailed,
            IsHideShortMusic = searchSetting.IsHideShortMusic,
        };
        string content = JsonSerializer.Serialize(request);

        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.UserConfig.WriteSearchConfig, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> WritePlaySettingAsync(PlaySetting playSetting)
    {
        var request = new PlaySettingRequest()
        {
            IsAutoNextWhenFailed = playSetting.IsAutoNextWhenFailed,
            IsCleanPlaylistWhenPlayMyFavorite = playSetting.IsCleanPlaylistWhenPlayMyFavorite,
            IsWifiPlayOnly = playSetting.IsWifiPlayOnly,
        };
        string content = JsonSerializer.Serialize(request);

        StringContent sc = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var response = await DataConfig.HttpClientWithToken.PostAsync(DataConfig.ApiSetting.UserConfig.WritePlayConfig, sc);
        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<Result>(json);
        if (obj == null || obj.Code != 0)
        {
            return false;
        }
        return true;
    }
}