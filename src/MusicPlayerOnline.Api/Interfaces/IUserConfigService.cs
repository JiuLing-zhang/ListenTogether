using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IUserConfigService
    {
        Task<UserSettingDto> ReadAllSettingAsync(int userId);
        Task<Result> WriteGeneralSettingAsync(int userId, GeneralSetting generalSetting);
        Task<Result> WriteSearchSettingAsync(int userId, SearchSetting searchSetting);
        Task<Result> WritePlaySettingAsync(int userId, PlaySetting playSetting);
    }
}
