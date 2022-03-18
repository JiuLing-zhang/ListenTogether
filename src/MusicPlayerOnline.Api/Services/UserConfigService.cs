using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.ErrorHandler;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model;
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

        public async Task<UserSettingDto> ReadAllSettingAsync(int userId)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (userConfig == null)
            {
                userConfig = await InitializationUserSetting(userId);
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
        private async Task<UserConfigEntity> InitializationUserSetting(int userId)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (userConfig != null)
            {
                return userConfig;
            }

            userConfig = new UserConfigEntity()
            {
                UserBaseId = userId,
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

        public async Task<Result> WriteGeneralSettingAsync(int userId, GeneralSetting generalSetting)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (userConfig == null)
            {
                return new Result(1, "原始配置不存在");
            }

            userConfig.GeneralSettingJson = JsonSerializer.Serialize(generalSetting);
            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(2, "保存失败");
            }

            return new Result(0, "保存成功");
        }

        public async Task<Result> WriteSearchSettingAsync(int userId, SearchSetting searchSetting)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (userConfig == null)
            {
                return new Result(1, "原始配置不存在");
            }

            userConfig.SearchSettingJson = JsonSerializer.Serialize(searchSetting);
            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(2, "保存失败");
            }
            return new Result(0, "保存成功");
        }

        public async Task<Result> WritePlaySettingAsync(int userId, PlaySetting playSetting)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (userConfig == null)
            {
                return new Result(1, "原始配置不存在");
            }

            userConfig.PlaySettingJson = JsonSerializer.Serialize(playSetting);
            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(2, "保存失败");
            }
            return new Result(0, "保存成功");
        }
    }
}
