using MusicPlayerOnline.Model;
using MusicPlayerOnline.Repository.Repositories;
using MusicPlayerOnline.Service.Interfaces;

namespace MusicPlayerOnline.Service.Services;
public class LocalTokenService : ILocalTokenService
{
    private readonly MusicRepository _repository;
    public LocalTokenService()
    {
        _repository = new MusicRepository();
    }

    public Task<TokenInfo> Read()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Write(TokenInfo token)
    {
        throw new NotImplementedException();
    }
}