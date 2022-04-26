using MusicPlayerOnline.Business.Interfaces;

namespace MusicPlayerOnline.Business.Factories;
public interface IPlaylistServiceFactory
{
    public IPlaylistService Create();
}