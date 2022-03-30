using MusicPlayerOnline.Business.Interfaces;

namespace MusicPlayerOnline.Business.Factories;
public interface IUserConfigServiceFactory
{
    public IUserConfigService Create();
}