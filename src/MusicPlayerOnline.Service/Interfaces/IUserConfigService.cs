using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Service.Interfaces
{
    public interface IUserConfigService
    {
        Task<UserSettingDto?> ReadAllSettingsAsync();
        Task<Result> WriteGeneralSettingAsync(GeneralSetting generalSetting);

        Task<Result> WriteSearchSettingAsync(SearchSetting searchSetting);

        Task<Result> WritePlaySettingAsync(PlaySetting playSetting);

        //Task WritePlayerConfigAsync(PlayerSetting playerSetting);
    }
}
