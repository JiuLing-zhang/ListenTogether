using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;
public interface IUserConfigService
{
    Task<UserSetting?> ReadAllSettingsAsync();
    Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting);

    Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting);

    Task<bool> WritePlaySettingAsync(PlaySetting playSetting);
}