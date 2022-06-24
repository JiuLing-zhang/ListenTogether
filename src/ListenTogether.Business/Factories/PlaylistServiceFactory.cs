using ListenTogether.Business.Interfaces;
using ListenTogether.Business.Services;
using ListenTogether.Data.Interfaces;
using ListenTogether.Data.Repositories.Api;
using ListenTogether.Data.Repositories.Local;

namespace ListenTogether.Business.Factories;

public class PlaylistServiceFactory : IPlaylistServiceFactory
{

    private readonly IEnumerable<IPlaylistRepository> _repositories;
    public PlaylistServiceFactory(IEnumerable<IPlaylistRepository> repositories)
    {
        _repositories = repositories;
    }

    public IPlaylistService Create()
    {
        var repositoryName = BusinessConfig.AppNetwork == Model.Enums.AppNetworkEnum.Online ? nameof(PlaylistApiRepository) : nameof(PlaylistLocalRepository);
        IPlaylistRepository repository = _repositories.First(x => x.GetType().Name == repositoryName);
        return new PlaylistService(repository);
    }
}