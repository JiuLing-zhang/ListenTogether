using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Interfaces;

public interface IEnvironmentConfigService
{
    Task<EnvironmentSetting> ReadAllSettingsAsync();
    Task WritePlayerSettingAsync(PlayerSetting playerSetting);
}
