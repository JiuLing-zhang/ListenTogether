using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Business.Services;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Data.Repositories.Api;
using MusicPlayerOnline.Data.Repositories.Local;

namespace MusicPlayerOnline.Business.Factories;

public class MusicServiceFactory : IMusicServiceFactory
{
    private readonly IEnumerable<IMusicRepository> _repositories;
    public MusicServiceFactory(IEnumerable<IMusicRepository> repositories)
    {
        _repositories = repositories;
    }

    public IMusicService Create()
    {
        var repositoryName = GlobalConfig.IsUseApiInterface ? nameof(MusicApiRepository) : nameof(MusicLocalRepository);
        IMusicRepository repository = _repositories.First(x => x.GetType().Name == repositoryName);
        return new MusicService(repository);
    }
}