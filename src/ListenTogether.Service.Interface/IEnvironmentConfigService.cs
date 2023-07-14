using ListenTogether.Model;

namespace ListenTogether.Data.Interface;

public interface IEnvironmentConfigService
{
    Task<EnvironmentSetting> ReadAllSettingsAsync();
    Task WritePlayerSettingAsync(PlayerSetting playerSetting);

    Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting);

    Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting);

    Task<bool> WritePlaySettingAsync(PlaySetting playSetting);
}
