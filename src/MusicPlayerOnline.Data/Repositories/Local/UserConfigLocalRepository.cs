using System.Text.Json;
using MusicPlayerOnline.Data.Entities;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Local;
public class UserConfigLocalRepository : IUserConfigRepository
{
    public UserSetting ReadAllSettings()
    {
        var userConfig = DatabaseProvide.Database.Table<UserConfigEntity>().FirstOrDefault();
        if (userConfig == null)
        {
            userConfig = InitializationUserSetting();
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
    private UserConfigEntity InitializationUserSetting()
    {
        var userConfig = DatabaseProvide.Database.Table<UserConfigEntity>().FirstOrDefault();
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

        var count = DatabaseProvide.Database.Insert(userConfig);
        if (count == 0)
        {
            throw new Exception("初始化用户配置失败");
        }
        return userConfig;
    }

    public bool WriteGeneralSetting(GeneralSetting generalSetting)
    {
        var userConfig = DatabaseProvide.Database.Table<UserConfigEntity>().FirstOrDefault();
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
        var userConfig = DatabaseProvide.Database.Table<UserConfigEntity>().FirstOrDefault();
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
        var userConfig = DatabaseProvide.Database.Table<UserConfigEntity>().FirstOrDefault();
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
