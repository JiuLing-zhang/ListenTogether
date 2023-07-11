using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interface;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;

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
    public async Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        return await _repository.WriteGeneralSettingAsync(generalSetting);
    }

    public async Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        return await _repository.WriteSearchSettingAsync(searchSetting);
    }

    public async Task<bool> WritePlaySettingAsync(PlaySetting playSetting)
    {
        return await _repository.WritePlaySettingAsync(playSetting);
    }
}