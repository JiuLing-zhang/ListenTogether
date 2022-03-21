using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Repository.Repositories;
using MusicPlayerOnline.Service.Interfaces;
namespace MusicPlayerOnline.Service.Services;
internal class UserConfigLocalService : IUserConfigService
{
    private readonly UserConfigRepository _repository;
    public UserConfigLocalService()
    {
        _repository = new UserConfigRepository();
    }

    public async Task<UserSettingDto?> ReadAllSettingsAsync()
    {
        return await _repository.ReadAllSettingAsync();
    }

    public async Task<Result> WriteGeneralSettingAsync(GeneralSetting generalSetting)
    {
        return await _repository.WriteGeneralSettingAsync(generalSetting);
    }

    public async Task<Result> WriteSearchSettingAsync(SearchSetting searchSetting)
    {
        return await _repository.WriteSearchSettingAsync(searchSetting);
    }

    public async Task<Result> WritePlaySettingAsync(PlaySetting playSetting)
    {
        return await _repository.WritePlaySettingAsync(playSetting);
    }
}