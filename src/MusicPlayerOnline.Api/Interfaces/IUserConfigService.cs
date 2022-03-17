using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Interfaces
{
    public interface IUserConfigService
    {
        Task<UserSettingDto> ReadAllConfigAsync(int userBaseId);
        Task<bool> WriteGeneralConfigAsync(int userBaseId, GeneralSetting generalSetting);
        Task<bool> WriteSearchConfigAsync(int userBaseId, SearchSetting searchSetting);
        Task<bool> WritePlayConfigAsync(int userBaseId, PlaySetting playSetting);
    }
}
