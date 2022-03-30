using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Interfaces;
public interface ITokenRepository
{
    public TokenInfo Read();
    public bool Write(TokenInfo token);
}