using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;

public interface IEnvironmentConfigRepository
{
    Task<EnvironmentSetting> ReadAllSettingsAsync();
    Task WritePlayerSettingAsync(PlayerSetting playerSetting);
}
