using System.Text.Json;
using MusicPlayerOnline.Data.Entities;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Local;

public class EnvironmentConfigLocalRepository : IEnvironmentConfigRepository
{
    public EnvironmentSetting ReadAllSettings()
    {
        var environmentConfig = DatabaseProvide.Database.Table<EnvironmentConfigEntity>().FirstOrDefault();
        if (environmentConfig == null)
        {
            environmentConfig = InitializationEnvironmentSetting();
        }

        var result = new EnvironmentSetting();
        var playerSetting = JsonSerializer.Deserialize<PlayerSetting>(environmentConfig.PlayerSettingJson) ?? throw new Exception("配置信息不存在：PlayerSettingJson");
        result.Player = new PlayerSetting()
        {
            Voice = playerSetting.Voice,
            IsSoundOff = playerSetting.IsSoundOff,
            PlayMode = playerSetting.PlayMode
        };

        //通用设置
        var generalConfig = JsonSerializer.Deserialize<GeneralSetting>(environmentConfig.GeneralSettingJson) ?? throw new Exception("配置信息不存在：GeneralSetting");
        result.General = new GeneralSetting()
        {
            IsAutoCheckUpdate = generalConfig.IsAutoCheckUpdate,
            IsDarkMode = generalConfig.IsDarkMode,
            IsHideWindowWhenMinimize = generalConfig.IsHideWindowWhenMinimize,
        };

        //播放设置
        var playConfig = JsonSerializer.Deserialize<PlaySetting>(environmentConfig.PlaySettingJson) ?? throw new Exception("配置信息不存在：PlaySetting");
        result.Play = new PlaySetting()
        {
            IsAutoNextWhenFailed = playConfig.IsAutoNextWhenFailed,
            IsCleanPlaylistWhenPlayMyFavorite = playConfig.IsCleanPlaylistWhenPlayMyFavorite,
            IsWifiPlayOnly = playConfig.IsWifiPlayOnly
        };

        //搜索设置
        var searchConfig = JsonSerializer.Deserialize<SearchSetting>(environmentConfig.SearchSettingJson) ?? throw new Exception("配置信息不存在：SearchSetting");
        result.Search = new SearchSetting()
        {
            EnablePlatform = searchConfig.EnablePlatform,
            IsCloseSearchPageWhenPlayFailed = searchConfig.IsCloseSearchPageWhenPlayFailed,
            IsHideShortMusic = searchConfig.IsHideShortMusic
        };

        return result;
    }

    private EnvironmentConfigEntity InitializationEnvironmentSetting()
    {
        var environmentConfig = DatabaseProvide.Database.Table<EnvironmentConfigEntity>().FirstOrDefault();
        if (environmentConfig != null)
        {
            return environmentConfig;
        }

        environmentConfig = new EnvironmentConfigEntity()
        {
            PlayerSettingJson = JsonSerializer.Serialize(new PlayerSetting()
            {
                Voice = 50,
                IsSoundOff = false,
                PlayMode = PlayModeEnum.RepeatList
            }),
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

        var count = DatabaseProvide.Database.Insert(environmentConfig);
        if (count == 0)
        {
            throw new Exception("初始化环境配置失败");
        }
        return environmentConfig;
    }

    public void WritePlayerSetting(PlayerSetting playerSetting)
    {
        var environmentConfig = DatabaseProvide.Database.Table<EnvironmentConfigEntity>().FirstOrDefault();
        if (environmentConfig == null)
        {
            throw new Exception("环境配置信息不存在，播放设置保存失败");
        }

        environmentConfig.PlayerSettingJson = JsonSerializer.Serialize(playerSetting);
        DatabaseProvide.Database.Update(environmentConfig);
    }

    public bool WriteGeneralSetting(GeneralSetting generalSetting)
    {
        var userConfig = DatabaseProvide.Database.Table<EnvironmentConfigEntity>().FirstOrDefault();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.GeneralSettingJson = JsonSerializer.Serialize(generalSetting);
        var count = DatabaseProvide.Database.Update(userConfig);
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public bool WriteSearchSetting(SearchSetting searchSetting)
    {
        var userConfig = DatabaseProvide.Database.Table<EnvironmentConfigEntity>().FirstOrDefault();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.SearchSettingJson = JsonSerializer.Serialize(searchSetting);
        var count = DatabaseProvide.Database.Update(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }

    public bool WritePlaySetting(PlaySetting playSetting)
    {
        var userConfig = DatabaseProvide.Database.Table<EnvironmentConfigEntity>().FirstOrDefault();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.PlaySettingJson = JsonSerializer.Serialize(playSetting);
        var count = DatabaseProvide.Database.Update(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }
}