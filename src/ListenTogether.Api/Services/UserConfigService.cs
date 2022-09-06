using ListenTogether.Api.DbContext;
using ListenTogether.Api.Entities;
using ListenTogether.Api.ErrorHandler;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;
using ListenTogether.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ListenTogether.Api.Services
{
    public class UserConfigService : IUserConfigService
    {
        private readonly DataContext _context;
        public UserConfigService(DataContext context)
        {
            _context = context;
        }

        public async Task<UserSettingResponse> ReadAllSettingAsync(int userId)
        {
            var userConfig = await _context.UserConfigs.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (userConfig == null)
            {
                userConfig = await InitializationUserSetting(userId);
            }

            var result = new UserSettingResponse();
            //通用设置
            var generalConfig = JsonSerializer.Deserialize<GeneralSettingRequest>(userConfig.GeneralSettingJson) ?? throw new AppException("配置信息不存在：GeneralSetting");
            result.General = new GeneralSettingResponse()
            {
                IsAutoCheckUpdate = generalConfig.IsAutoCheckUpdate,
                IsHideWindowWhenMinimize = generalConfig.IsHideWindowWhenMinimize,
            };

            //播放设置
            var playConfig = JsonSerializer.Deserialize<PlaySettingRequest>(userConfig.PlaySettingJson) ?? throw new AppException("配置信息不存在：PlaySetting");
            result.Play = new PlaySettingResponse()
            {
                IsAutoNextWhenFailed = playConfig.IsAutoNextWhenFailed,
                IsCleanPlaylistWhenPlayMyFavorite = playConfig.IsCleanPlaylistWhenPlayMyFavorite,
                IsWifiPlayOnly = playConfig.IsWifiPlayOnly
            };

            //搜索设置
            var searchConfig = JsonSerializer.Deserialize<SearchSettingRequest>(userConfig.SearchSettingJson) ?? throw new AppException("配置信息不存在：SearchSetting");
            result.Search = new SearchSettingResponse()
            {
                EnablePlatform = (int)searchConfig.EnablePlatform,
                IsCloseSearchPageWhenPlayFailed = searchConfig.IsCloseSearchPageWhenPlayFailed,
                IsHideShortMusic = searchConfig.IsHideShortMusic,
                IsPlayWhenAddToFavorite = searchConfig.IsPlayWhenAddToFavorite
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

            PlatformEnum platform = PlatformEnum.NetEase | PlatformEnum.KuGou | PlatformEnum.MiGu;
            userConfig = new UserConfigEntity()
            {
                UserBaseId = userId,
                GeneralSettingJson = JsonSerializer.Serialize(new GeneralSettingRequest()
                {
                    IsAutoCheckUpdate = true,
                    IsHideWindowWhenMinimize = true
                }),
                SearchSettingJson = JsonSerializer.Serialize(new SearchSettingRequest()
                {
                    EnablePlatform = (int)platform,
                    IsHideShortMusic = true,
                    IsCloseSearchPageWhenPlayFailed = false,
                    IsPlayWhenAddToFavorite = false
                }),
                PlaySettingJson = JsonSerializer.Serialize(new PlaySettingRequest()
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

        public async Task<Result> WriteGeneralSettingAsync(int userId, GeneralSettingRequest generalSetting)
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

        public async Task<Result> WriteSearchSettingAsync(int userId, SearchSettingRequest searchSetting)
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

        public async Task<Result> WritePlaySettingAsync(int userId, PlaySettingRequest playSetting)
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
