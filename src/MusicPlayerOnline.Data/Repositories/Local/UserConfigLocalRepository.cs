using System.Text.Json;
using MusicPlayerOnline.Data.Entities;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Local;
public class UserConfigLocalRepository : IUserConfigRepository
{
    public async Task<UserSetting> ReadAllSettingsAsync()
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            userConfig = await InitializationUserSetting();
        }

        var result = new UserSetting();
        //通用设置
        var generalConfig = JsonSerializer.Deserialize<GeneralSetting>(userConfig.GeneralSettingJson) ?? throw new Exception("配置信息不存在：GeneralSetting");
        result.General = new GeneralSetting()
        {
            IsAutoCheckUpdate = generalConfig.IsAutoCheckUpdate,
            IsHideWindowWhenMinimize = generalConfig.IsHideWindowWhenMinimize,
        };

        //播放设置
        var playConfig = JsonSerializer.Deserialize<PlaySetting>(userConfig.PlaySettingJson) ?? throw new Exception("配置信息不存在：PlaySetting");
        result.Play = new PlaySetting()
        {
            IsAutoNextWhenFailed = playConfig.IsAutoNextWhenFailed,
            IsCleanPlaylistWhenPlayMyFavorite = playConfig.IsCleanPlaylistWhenPlayMyFavorite,
            IsWifiPlayOnly = playConfig.IsWifiPlayOnly
        };

        //搜索设置
        var searchConfig = JsonSerializer.Deserialize<SearchSetting>(userConfig.SearchSettingJson) ?? throw new Exception("配置信息不存在：SearchSetting");
        result.Search = new SearchSetting()
        {
            EnablePlatform = searchConfig.EnablePlatform,
            IsCloseSearchPageWhenPlayFailed = searchConfig.IsCloseSearchPageWhenPlayFailed,
            IsHideShortMusic = searchConfig.IsHideShortMusic
        };

        return result;
    }

    /// <summary>
    /// 初始化用户配置
    /// </summary>
    private async Task<UserConfigEntity> InitializationUserSetting()
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
        if (userConfig != null)
        {
            return userConfig;
        }

        userConfig = new UserConfigEntity()
        {
            GeneralSettingJson = JsonSerializer.Serialize(new GeneralSetting()
            {
                IsAutoCheckUpdate = true,
                IsHideWindowWhenMinimize = true
            }),
            SearchSettingJson = JsonSerializer.Serialize(new SearchSetting()
            {
                EnablePlatform = PlatformEnum.NetEase | PlatformEnum.KuGou | PlatformEnum.MiGu,
                IsHideShortMusic = true,
                IsCloseSearchPageWhenPlayFailed = false
            }),
            PlaySettingJson = JsonSerializer.Serialize(new PlaySetting()
            {
                IsAutoNextWhenFailed = true,
                IsCleanPlaylistWhenPlayMyFavorite = true,
                IsWifiPlayOnly = true
            })
        };

        var count = await DatabaseProvide.DatabaseAsync.InsertAsync(userConfig);
        if (count == 0)
        {
            throw new Exception("初始化用户配置失败");
        }
        return userConfig;
    }

    public async Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.GeneralSettingJson = JsonSerializer.Serialize(generalSetting);
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.SearchSettingJson = JsonSerializer.Serialize(searchSetting);
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> WritePlaySettingAsync(PlaySetting playSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.PlaySettingJson = JsonSerializer.Serialize(playSetting);
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }
}
