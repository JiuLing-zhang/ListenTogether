using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class EnvironmentConfigService : IEnvironmentConfigService
{
    private readonly IEnvironmentConfigRepository _repository;
    public EnvironmentConfigService(IEnvironmentConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<EnvironmentSetting> ReadAllSettingsAsync()
    {
        return await _repository.ReadAllSettingsAsync();
    }

    public async Task WritePlayerSettingAsync(PlayerSetting playerSetting)
    {
        await _repository.WritePlayerSettingAsync(playerSetting);
    }
}