using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class EnvironmentConfigService : IEnvironmentConfigService
{
    public Task<EnvironmentSetting> ReadAllSettingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task WritePlayerSettingAsync(PlayerSetting playerSetting)
    {
        throw new NotImplementedException();
    }
}