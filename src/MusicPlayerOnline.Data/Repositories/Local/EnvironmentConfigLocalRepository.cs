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
                IsHideShortMusic = true,
                IsCloseSearchPageWhenPlayFailed = false
            }).ToJson(),
            PlaySettingJson = (new PlaySetting()
            {
                IsAutoNextWhenFailed = true,
                IsCleanPlaylistWhenPlayMyFavorite = true,
                IsWifiPlayOnly = true
            }).ToJson()
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

        environmentConfig.PlayerSettingJson = playerSetting.ToJson();
        DatabaseProvide.Database.Update(environmentConfig);
    }

    public bool WriteGeneralSetting(GeneralSetting generalSetting)
    {
        var userConfig = DatabaseProvide.Database.Table<EnvironmentConfigEntity>().FirstOrDefault();
        if (userConfig == null)
        {
            return false;
        }

        userConfig.GeneralSettingJson = generalSetting.ToJson();
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

        userConfig.SearchSettingJson = searchSetting.ToJson();
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

        userConfig.PlaySettingJson = playSetting.ToJson();
        var count = DatabaseProvide.Database.Update(userConfig);
        if (count == 0)
        {
            return false;
        }
        return true;
    }
}