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
    public async Task<UserSetting?> ReadAllSettingsAsync()
    {
        return await _repository.ReadAllSettingsAsync();
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