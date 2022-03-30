using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Business.Services;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Data.Repositories.Api;
using MusicPlayerOnline.Data.Repositories.Local;

namespace MusicPlayerOnline.Business.Factories;

public class PlaylistServiceFactory : IPlaylistServiceFactory
{

    private readonly IEnumerable<IPlaylistRepository> _repositories;
    public PlaylistServiceFactory(IEnumerable<IPlaylistRepository> repositories)
    {
        _repositories = repositories;
    }

    public IPlaylistService Create()
    {
        var repositoryName = BusinessConfig.IsUseApiInterface ? nameof(PlaylistApiRepository) : nameof(PlaylistLocalRepository);
        IPlaylistRepository repository = _repositories.First(x => x.GetType().Name == repositoryName);
        return new PlaylistService(repository);
    }
}