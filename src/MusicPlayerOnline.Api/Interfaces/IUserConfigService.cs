using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IUserConfigService
    {
        Task<UserSettingDto> ReadAllConfigAsync(int userId);
        Task<Result> WriteGeneralConfigAsync(int userId, GeneralSetting generalSetting);
        Task<Result> WriteSearchConfigAsync(int userId, SearchSetting searchSetting);
        Task<Result> WritePlayConfigAsync(int userId, PlaySetting playSetting);
    }
}
