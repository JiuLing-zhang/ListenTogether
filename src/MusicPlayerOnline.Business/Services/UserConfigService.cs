using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class UserConfigService : IUserConfigService
{

    private readonly IUserConfigRepository _repository;
    public UserConfigService(IUserConfigRepository repository)
    {
        _repository = repository;
    }
    public UserSetting? ReadAllSettings()
    {
        return _repository.ReadAllSettings();
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