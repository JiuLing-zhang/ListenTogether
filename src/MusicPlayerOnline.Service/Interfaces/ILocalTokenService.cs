using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Service.Interfaces;
public interface ILocalTokenService
{
    public Task<TokenInfo> Read();
    public Task<bool> Write(TokenInfo token);
}