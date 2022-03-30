using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Business.Services;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Data.Repositories.Api;
using MusicPlayerOnline.Data.Repositories.Local;

namespace MusicPlayerOnline.Business.Factories;

public class MyFavoriteServiceFactory : IMyFavoriteServiceFactory
{

    private readonly IEnumerable<IMyFavoriteRepository> _repositories;
    public MyFavoriteServiceFactory(IEnumerable<IMyFavoriteRepository> repositories)
    {
        _repositories = repositories;
    }

    public IMyFavoriteService Create()
    {
        var repositoryName = GlobalConfig.IsUseApiInterface ? nameof(MyFavoriteApiRepository) : nameof(MyFavoriteLocalRepository);
        IMyFavoriteRepository repository = _repositories.First(x => x.GetType().Name == repositoryName);
        return new MyFavoriteService(repository);
    }
}