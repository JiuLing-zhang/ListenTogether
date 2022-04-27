using System.Text.Json;
using ListenTogether.Data.Entities;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Data.Repositories.Local;

public class EnvironmentConfigLocalRepository : IEnvironmentConfigRepository
{
    public async Task<EnvironmentSetting> ReadAllSettingsAsync()
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig == null)
        {
            environmentConfig = await InitializationEnvironmentSettingAsync();
        }

        var result = new EnvironmentSetting();
        var playerSetting = environmentConfig.PlayerSettingJson.ToObject<PlayerSetting>() ?? throw new Exception("配置信息不存在：PlayerSettingJson");
        result.Player = new PlayerSetting()
        {
            Volume = playerSetting.Volume,
            IsSoundOff = playerSetting.IsSoundOff,
            PlayMode = playerSetting.PlayMode
        };

        //通用设置
        var generalConfig = environmentConfig.GeneralSettingJson.ToObject<GeneralSetting>() ?? throw new Exception("配置信息不存在：GeneralSetting");
        result.General = new GeneralSetting()
        {
            IsAutoCheckUpdate = generalConfig.IsAutoCheckUpdate,
            IsDarkMode = generalConfig.IsDarkMode,
            IsHideWindowWhenMinimize = generalConfig.IsHideWindowWhenMinimize,
        };

        //播放设置
        var playConfig = environmentConfig.PlaySettingJson.ToObject<PlaySetting>() ?? throw new Exception("配置信息不存在：PlaySetting");
        result.Play = new PlaySetting()
        {
            IsAutoNextWhenFailed = playConfig.IsAutoNextWhenFailed,
            IsCleanPlaylistWhenPlayMyFavorite = playConfig.IsCleanPlaylistWhenPlayMyFavorite,
            IsWifiPlayOnly = playConfig.IsWifiPlayOnly
        };

        //搜索设置
        var searchConfig = environmentConfig.SearchSettingJson.ToObject<SearchSetting>() ?? throw new Exception("配置信息不存在：SearchSetting");
        result.Search = new SearchSetting()
        {
            EnablePlatform = searchConfig.EnablePlatform,
            IsHideShortMusic = searchConfig.IsHideShortMusic
        };

        return result;
    }

    private async Task<EnvironmentConfigEntity> InitializationEnvironmentSettingAsync()
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig != null)
        {
            return environmentConfig;
        }

        environmentConfig = new EnvironmentConfigEntity()
        {
            PlayerSettingJson = (new PlayerSetting()
            {
                Volume = 50,
                IsSoundOff = false,
                PlayMode = PlayModeEnum.RepeatList
            }).ToJson(),
            GeneralSettingJson = (new GeneralSetting()
            {
                IsAutoCheckUpdate = true,
                IsHideWindowWhenMinimize = true
            }).ToJson(),
            SearchSettingJson = (new SearchSetting()
            {
                EnablePlatform = PlatformEnum.NetEase | PlatformEnum.KuGou | PlatformEnum.MiGu,
                IsHideShortMusic = true
            }).ToJson(),
            PlaySettingJson = (new PlaySetting()
            {
                IsAutoNextWhenFailed = true,
                IsCleanPlaylistWhenPlayMyFavorite = true,
                IsWifiPlayOnly = true
            }).ToJson()
        };

        var count = await DatabaseProvide.DatabaseAsync.InsertAsync(environmentConfig);
        if (count == 0)
        {
            throw new Exception("初始化环境配置失败");
        }
        return environmentConfig;
    }

    public async Task WritePlayerSettingAsync(PlayerSetting playerSetting)
    {
        var environmentConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (environmentConfig == null)
        {
            throw new Exception("环境配置信息不存在，播放设置保存失败");
        }

        environmentConfig.PlayerSettingJson = playerSetting.ToJson();
        await DatabaseProvide.DatabaseAsync.UpdateAsync(environmentConfig);
    }

    public async Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.GeneralSettingJson = generalSetting.ToJson();
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.SearchSettingJson = searchSetting.ToJson();
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> WritePlaySettingAsync(PlaySetting playSetting)
    {
        var userConfig = await DatabaseProvide.DatabaseAsync.Table<EnvironmentConfigEntity>().FirstOrDefaultAsync();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.PlaySettingJson = playSetting.ToJson();
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }
}