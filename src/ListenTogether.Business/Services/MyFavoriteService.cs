using ListenTogether.Business.Interfaces;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Business.Services;

public class MyFavoriteService : IMyFavoriteService
{
    private readonly IMyFavoriteRepository _repository;
    public MyFavoriteService(IMyFavoriteRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 歌单名称不可用的关键字
    /// </summary>
    private List<string> UnavailableName = new List<string>()
    {
        "确定",
        "取消",
        "创建一个新歌单"
    };

    public async Task<MyFavorite?> GetOneAsync(int id)
    {
        return await _repository.GetOneAsync(id);
    }

    public async Task<List<MyFavorite>?> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<bool> NameExistAsync(string myFavoriteName)
    {
        return await _repository.NameExistAsync(myFavoriteName);
    }

    public async Task<MyFavorite?> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        if (UnavailableName.Contains(myFavorite.Name))
        {
            return default;
        }
        return await _repository.AddOrUpdateAsync(myFavorite);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        return await _repository.RemoveAsync(id);
    }

    public async Task<bool> AddMusicToMyFavoriteAsync(int id, LocalMusic music)
    {
        return await _repository.AddMusicToMyFavoriteAsync(id, music);
    }

    public async Task<List<MyFavoriteDetail>?> GetMyFavoriteDetailAsync(int id)
    {
        return await _repository.GetMyFavoriteDetailAsync(id);
    }

    public async Task<bool> RemoveDetailAsync(int id)
    {
        return await _repository.RemoveDetailAsync(id);
    }
}