using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Service.Interfaces
{
    public interface IConfigService
    {
        Task<UserSettingDto> ReadAllConfigAsync(); 
        Task WriteGeneralConfigAsync(GeneralSetting generalSetting);
         
        Task WritePlatformConfigAsync(SearchSetting searchSetting);
         
        Task WritePlayConfigAsync(PlaySetting playSetting);
         
        //Task WritePlayerConfigAsync(PlayerSetting playerSetting);
    }
}
