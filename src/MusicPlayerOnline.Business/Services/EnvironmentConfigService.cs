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

    public EnvironmentSetting ReadAllSettings()
    {
        return _repository.ReadAllSettings();
    }

    public void WritePlayerSetting(PlayerSetting playerSetting)
    {
        _repository.WritePlayerSetting(playerSetting);
    }
}