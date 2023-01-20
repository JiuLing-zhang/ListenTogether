﻿using ListenTogether.Model;

namespace ListenTogether.Data.Interfaces;
public interface IMyFavoriteRepository
{
    Task<MyFavorite?> GetOneAsync(int id);
    Task<List<MyFavorite>?> GetAllAsync();
    Task<bool> NameExistAsync(string myFavoriteName);
    Task<MyFavorite?> AddOrUpdateAsync(MyFavorite myFavorite);
    Task<bool> RemoveAsync(int id);

    Task<bool> AddMusicToMyFavoriteAsync(int id, LocalMusic music);
    Task<List<MyFavoriteDetail>?> GetMyFavoriteDetailAsync(int id);
    Task<bool> RemoveDetailAsync(int id);
}