using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;

public interface IEnvironmentConfigRepository
{
    Task<EnvironmentSetting> ReadAllSettingsAsync();
    Task WritePlayerSettingAsync(PlayerSetting playerSetting);

    Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting);

    Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting);

    Task<bool> WritePlaySettingAsync(PlaySetting playSetting);
}
