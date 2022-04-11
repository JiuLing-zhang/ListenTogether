using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class UserLocalService : IUserLocalService
{
    private readonly IUserLocalRepository _repository;
    public UserLocalService(IUserLocalRepository repository)
    {
        _repository = repository;
    }
    public User? Read()
    {
        return _repository.Read();
    }

    public void Remove()
    {
        _repository.Remove();
    }

    public void UpdateToken(TokenInfo tokenInfo)
    {
        _repository.UpdateToken(tokenInfo);
    }

    public bool Write(User user)
    {
        return _repository.Write(user);
    }
}

