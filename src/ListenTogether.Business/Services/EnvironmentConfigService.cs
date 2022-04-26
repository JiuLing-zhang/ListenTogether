using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;

public class EnvironmentConfigService : IEnvironmentConfigService
{
    private readonly IEnvironmentConfigRepository _repository;
    public EnvironmentConfigService(IEnvironmentConfigRepository repository)
    {
        _repository = repository;
    }

    public EnvironmentSetting ReadAllSettings()
    {
        return _repository.ReadAllSettings();
    }

    public void WritePlayerSetting(PlayerSetting playerSetting)
    {
        _repository.WritePlayerSetting(playerSetting);
    }
    public bool WriteGeneralSetting(GeneralSetting generalSetting)
    {
        return _repository.WriteGeneralSetting(generalSetting);
    }

    public bool WriteSearchSetting(SearchSetting searchSetting)
    {
        return _repository.WriteSearchSetting(searchSetting);
    }

    public bool WritePlaySetting(PlaySetting playSetting)
    {
        return _repository.WritePlaySetting(playSetting);
    }
}