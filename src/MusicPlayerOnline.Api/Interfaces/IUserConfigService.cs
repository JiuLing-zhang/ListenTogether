using MusicPlayerOnline.Model.Request;
using MusicPlayerOnline.Model.Response;

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
