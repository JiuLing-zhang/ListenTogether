using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Business.Services;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Data.Repositories.Api;
using MusicPlayerOnline.Data.Repositories.Local;

namespace MusicPlayerOnline.Business.Factories;

public class UserConfigServiceFactory : IUserConfigServiceFactory
{

    private readonly IEnumerable<IUserConfigRepository> _repositories;
    public UserConfigServiceFactory(IEnumerable<IUserConfigRepository> repositories)
    {
        _repositories = repositories;
    }

    public IUserConfigService Create()
    {
        //TODO 只有本地库
        var repositoryName = BusinessConfig.IsUseApiInterface ? nameof(UserConfigLocalRepository) : nameof(UserConfigLocalRepository);
        IUserConfigRepository repository = _repositories.First(x => x.GetType().Name == repositoryName);
        return new UserConfigService(repository);
    }
}