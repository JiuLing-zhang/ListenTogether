using ListenTogether.Business.Interfaces;
using ListenTogether.Business.Services;
using ListenTogether.Data.Interfaces;
using ListenTogether.Data.Repositories.Api;
using ListenTogether.Data.Repositories.Local;

namespace ListenTogether.Business.Factories;

public class MyFavoriteServiceFactory : IMyFavoriteServiceFactory
{

    private readonly IEnumerable<IMyFavoriteRepository> _repositories;
    public MyFavoriteServiceFactory(IEnumerable<IMyFavoriteRepository> repositories)
    {
        _repositories = repositories;
    }

    public IMyFavoriteService Create()
    {
        var repositoryName = BusinessConfig.IsUseApiInterface ? nameof(MyFavoriteApiRepository) : nameof(MyFavoriteLocalRepository);
        IMyFavoriteRepository repository = _repositories.First(x => x.GetType().Name == repositoryName);
        return new MyFavoriteService(repository);
    }
}