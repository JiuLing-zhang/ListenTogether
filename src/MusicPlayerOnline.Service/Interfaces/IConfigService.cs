using MusicPlayerOnline.Model.ApiResponse;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Service
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
