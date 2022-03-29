using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface IUserConfigRepository
{
    Task<UserSetting> ReadAllSettingsAsync();
    Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting);

    Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting);

    Task<bool> WritePlaySettingAsync(PlaySetting playSetting);
}