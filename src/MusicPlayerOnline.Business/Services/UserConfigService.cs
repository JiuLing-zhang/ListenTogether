using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class UserConfigService : IUserConfigService
{
    public Task<UserSetting?> ReadAllSettingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        throw new NotImplementedException();
    }

    public Task<bool> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        throw new NotImplementedException();
    }

    public Task<bool> WritePlaySettingAsync(PlaySetting playSetting)
    {
        throw new NotImplementedException();
    }
}