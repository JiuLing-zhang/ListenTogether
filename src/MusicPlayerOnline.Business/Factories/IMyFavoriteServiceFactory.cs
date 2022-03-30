using MusicPlayerOnline.Business.Interfaces;

namespace MusicPlayerOnline.Business.Factories;
public interface IMyFavoriteServiceFactory
{
    public IMyFavoriteService Create();
}