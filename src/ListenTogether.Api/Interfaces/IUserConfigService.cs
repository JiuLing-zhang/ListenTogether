using ListenTogether.Api.Models;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;

namespace ListenTogether.Api.Interfaces
{
    public interface IUserConfigService
    {
        Task<UserSettingResponse> ReadAllSettingAsync(int userId);
        Task<Result> WriteGeneralSettingAsync(int userId, GeneralSettingRequest generalSetting);
        Task<Result> WriteSearchSettingAsync(int userId, SearchSettingRequest searchSetting);
        Task<Result> WritePlaySettingAsync(int userId, PlaySettingRequest playSetting);
    }
}
