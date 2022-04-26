using ListenTogether.Business.Interfaces;

namespace ListenTogether.Business.Factories;
public interface IPlaylistServiceFactory
{
    public IPlaylistService Create();
}