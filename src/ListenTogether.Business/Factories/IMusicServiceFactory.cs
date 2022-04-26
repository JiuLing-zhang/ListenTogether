using MusicPlayerOnline.Business.Interfaces;

namespace MusicPlayerOnline.Business.Factories;
public interface IMusicServiceFactory
{
    public IMusicService Create();
}