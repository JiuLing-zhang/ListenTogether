using ListenTogether.Business.Interfaces;

namespace ListenTogether.Business.Factories;
public interface IMusicServiceFactory
{
    public IMusicService Create();
}