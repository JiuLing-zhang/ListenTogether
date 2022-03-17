using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.ErrorHandler;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Api.Services
{
    public class UserConfigService : IUserConfigService
    {
        private readonly DataContext _context;
        public UserConfigService(DataContext context)
        {
            _context = context;
        }

        public async Task<UserSettingDto> ReadAllConfigAsync(int userBaseId)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userBaseId);
            if (userConfig == null)
            {
                userConfig = await InitUserConfig(userBaseId);
            }

            var result = new UserSettingDto();
            //通用设置
            var generalConfig = JsonSerializer.Deserialize<GeneralSetting>(userConfig.GeneralSettingJson) ?? throw new AppException("配置信息不存在：GeneralSetting");
            result.General = new GeneralSettingDto()
            {
                IsAutoCheckUpdate = generalConfig.IsAutoCheckUpdate,
                IsHideWindowWhenMinimize = generalConfig.IsHideWindowWhenMinimize,
            };

            //播放设置
            var playConfig = JsonSerializer.Deserialize<PlaySetting>(userConfig.PlaySettingJson) ?? throw new AppException("配置信息不存在：PlaySetting");
            result.Play = new PlaySettingDto()
            {
                IsAutoNextWhenFailed = playConfig.IsAutoNextWhenFailed,
                IsCleanPlaylistWhenPlayMyFavorite = playConfig.IsCleanPlaylistWhenPlayMyFavorite,
                IsWifiPlayOnly = playConfig.IsWifiPlayOnly
            };

            //搜索设置
            var searchConfig = JsonSerializer.Deserialize<SearchSetting>(userConfig.SearchSettingJson) ?? throw new AppException("配置信息不存在：SearchSetting");
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
        private async Task<UserConfigEntity> InitUserConfig(int userBaseId)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userBaseId);
            if (userConfig != null)
            {
                return userConfig;
            }

            userConfig = new UserConfigEntity()
            {
                UserBaseId = userBaseId,
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
            await _context.UserConfigs.AddAsync(userConfig);
            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new AppException("初始化用户配置失败");
            }
            return userConfig;
        }

        public async Task<bool> WriteGeneralConfigAsync(int userBaseId, GeneralSetting generalSetting)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userBaseId);
            if (userConfig == null)
            {
                return false;
            }

            userConfig.GeneralSettingJson = JsonSerializer.Serialize(generalSetting);
            var count = await _context.SaveChangesAsync();
            return count != 0;
        }

        public async Task<bool> WriteSearchConfigAsync(int userBaseId, SearchSetting searchSetting)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userBaseId);
            if (userConfig == null)
            {
                return false;
            }

            userConfig.SearchSettingJson = JsonSerializer.Serialize(searchSetting);
            var count = await _context.SaveChangesAsync();
            return count != 0;
        }

        public async Task<bool> WritePlayConfigAsync(int userBaseId, PlaySetting playSetting)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userBaseId);
            if (userConfig == null)
            {
                return false;
            }

            userConfig.PlaySettingJson = JsonSerializer.Serialize(playSetting);
            var count = await _context.SaveChangesAsync();
            return count != 0;
        }

    }
}
