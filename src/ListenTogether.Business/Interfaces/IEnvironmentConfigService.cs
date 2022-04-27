using ListenTogether.Model;

namespace ListenTogether.Business.Interfaces;

public interface IEnvironmentConfigService
{
    Task<EnvironmentSetting> ReadAllSettingsAsync();
    Task WritePlayerSettingAsync(PlayerSetting playerSetting);
    Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting);

    Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting);

    Task<bool> WritePlaySettingAsync(PlaySetting playSetting);
}
