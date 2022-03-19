using System.Text.Json;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Repository.Entities;

namespace MusicPlayerOnline.Repository.Repositories
{
    public class UserConfigRepository
    {
        public async Task<UserSettingDto> ReadAllSettingAsync()
        {
            var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
            if (userConfig == null)
            {
                userConfig = await InitializationUserSetting();
            }

            var result = new UserSettingDto();
            //通用设置
            var generalConfig = JsonSerializer.Deserialize<GeneralSetting>(userConfig.GeneralSettingJson) ?? throw new Exception("配置信息不存在：GeneralSetting");
            result.General = new GeneralSettingDto()
            {
                IsAutoCheckUpdate = generalConfig.IsAutoCheckUpdate,
                IsHideWindowWhenMinimize = generalConfig.IsHideWindowWhenMinimize,
            };

            //播放设置
            var playConfig = JsonSerializer.Deserialize<PlaySetting>(userConfig.PlaySettingJson) ?? throw new Exception("配置信息不存在：PlaySetting");
            result.Play = new PlaySettingDto()
            {
                IsAutoNextWhenFailed = playConfig.IsAutoNextWhenFailed,
                IsCleanPlaylistWhenPlayMyFavorite = playConfig.IsCleanPlaylistWhenPlayMyFavorite,
                IsWifiPlayOnly = playConfig.IsWifiPlayOnly
            };

            //搜索设置
            var searchConfig = JsonSerializer.Deserialize<SearchSetting>(userConfig.SearchSettingJson) ?? throw new Exception("配置信息不存在：SearchSetting");
            result.Search = new SearchSettingDto()
            {
                EnablePlatform = (int)searchConfig.EnablePlatform,
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

        public async Task<Result> WriteGeneralSettingAsync(GeneralSetting generalSetting)
        {
            var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
            if (userConfig == null)
            {
                return new Result(1, "原始配置不存在");
            }

            userConfig.GeneralSettingJson = JsonSerializer.Serialize(generalSetting);
            var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
            if (count == 0)
            {
                return new Result(2, "保存失败");
            }

            return new Result(0, "保存成功");
        }

        public async Task<Result> WriteSearchSettingAsync(SearchSetting searchSetting)
        {
            var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
            if (userConfig == null)
            {
                return new Result(1, "原始配置不存在");
            }

            userConfig.SearchSettingJson = JsonSerializer.Serialize(searchSetting);
            var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
            if (count == 0)
            {
                return new Result(2, "保存失败");
            }
            return new Result(0, "保存成功");
        }

        public async Task<Result> WritePlaySettingAsync(PlaySetting playSetting)
        {
            var userConfig = await DatabaseProvide.DatabaseAsync.Table<UserConfigEntity>().FirstOrDefaultAsync();
            if (userConfig == null)
            {
                return new Result(1, "原始配置不存在");
            }

            userConfig.PlaySettingJson = JsonSerializer.Serialize(playSetting);
            var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(userConfig);
            if (count == 0)
            {
                return new Result(2, "保存失败");
            }
            return new Result(0, "保存成功");
        }
    }
}
