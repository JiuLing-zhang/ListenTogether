using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface ITokenRepository
{
    public Task<TokenInfo> Read();
    public Task<bool> Write(TokenInfo token);
}