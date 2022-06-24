using ListenTogether.Business.Interfaces;
using ListenTogether.Business.Services;
using ListenTogether.Data.Interfaces;
using ListenTogether.Data.Repositories.Api;
using ListenTogether.Data.Repositories.Local;

namespace ListenTogether.Business.Factories;

public class MusicServiceFactory : IMusicServiceFactory
{
    private readonly IEnumerable<IMusicRepository> _repositories;
    public MusicServiceFactory(IEnumerable<IMusicRepository> repositories)
    {
        _repositories = repositories;
    }

    public IMusicService Create()
    {
        var repositoryName = BusinessConfig.AppNetwork == Model.Enums.AppNetworkEnum.Online ? nameof(MusicApiRepository) : nameof(MusicLocalRepository);
        IMusicRepository repository = _repositories.First(x => x.GetType().Name == repositoryName);
        return new MusicService(repository);
    }
}