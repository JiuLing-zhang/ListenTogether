using MusicPlayerOnline.Api.Models;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IUserConfigService
    {
        Task<UserSettingResponse> ReadAllSettingAsync(int userId);
        Task<Result> WriteGeneralSettingAsync(int userId, GeneralSettingRequest generalSetting);
        Task<Result> WriteSearchSettingAsync(int userId, SearchSettingRequest searchSetting);
        Task<Result> WritePlaySettingAsync(int userId, PlaySettingRequest playSetting);
    }
}
