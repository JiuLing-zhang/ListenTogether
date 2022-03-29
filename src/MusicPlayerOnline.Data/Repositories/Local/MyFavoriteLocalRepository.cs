using MusicPlayerOnline.Data.Entities;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Repositories.Local;
public class MyFavoriteLocalRepository : IMyFavoriteRepository
{
    public async Task<MyFavorite?> GetOneAsync(int id)
    {
        var myFavorite = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (myFavorite == null)
        {
            return default;
        }

        int musicCount = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetailEntity>().CountAsync(x => x.MyFavoriteId == id);

        return new MyFavorite()
        {
            Id = myFavorite.Id,
            Name = myFavorite.Name,
            ImageUrl = myFavorite.ImageUrl,
            MusicCount = musicCount
        };
    }

    public async Task<List<MyFavorite>?> GetAllAsync()
    {
        string sql = $"SELECT Id,Name,ImageUrl,(SELECT COUNT(*) FROM MyFavoriteDetail mfd WHERE MyFavoriteId=mf.Id)AS MusicCount FROM MyFavorite mf";
        var myFavorites = await DatabaseProvide.DatabaseAsync.QueryAsync<MyFavoriteEntity>(sql);
        return myFavorites.Select(x => new MyFavorite()
        {
            Id = x.Id,
            ImageUrl = x.ImageUrl,
            MusicCount = x.MusicCount,
            Name = x.Name
        }).ToList();
    }
    public async Task<bool> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        var favorite = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteEntity>().FirstOrDefaultAsync(x => x.Id == myFavorite.Id);
        int count;
        if (favorite == null)
        {
            favorite = new MyFavoriteEntity()
            {
                Id = myFavorite.Id,
                Name = myFavorite.Name,
                ImageUrl = myFavorite.ImageUrl
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(favorite);
        }
        else
        {
            favorite.Id = myFavorite.Id;
            favorite.Name = myFavorite.Name;
            favorite.ImageUrl = myFavorite.ImageUrl;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(favorite);
        }
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        await DatabaseProvide.DatabaseAsync.DeleteAsync<MyFavoriteEntity>(id);
        return true;
    }
    public async Task<bool> AddMusicToMyFavorite(int id, MyFavoriteDetail myFavoriteDetail)
    {
        var favorite = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (favorite == null)
        {
            return false;
        }

        var favoriteDetail = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetailEntity>().FirstOrDefaultAsync(x => x.MusicId == myFavoriteDetail.MusicId);
        int count;
        if (favoriteDetail == null)
        {
            favoriteDetail = new MyFavoriteDetailEntity()
            {
                Platform = (int)myFavoriteDetail.Platform,
                MyFavoriteId = id,
                MusicName = myFavoriteDetail.MusicName,
                MusicId = myFavoriteDetail.MusicId,
                MusicAlbum = myFavoriteDetail.MusicAlbum,
                MusicArtist = myFavoriteDetail.MusicArtist
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(favoriteDetail);
        }
        else
        {
            favoriteDetail.MusicId = myFavoriteDetail.MusicId;
            favoriteDetail.MusicName = myFavoriteDetail.MusicName;
            favoriteDetail.MusicArtist = myFavoriteDetail.MusicArtist;
            favoriteDetail.MusicAlbum = myFavoriteDetail.MusicAlbum;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(favoriteDetail);
        }
        if (count == 0)
        {
            return false;
        }
        return true;

    }

    public async Task<List<MyFavoriteDetail>?> GetMyFavoriteDetail(int id)
    {
        var detailList = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetailEntity>().Where(x => x.MyFavoriteId == id).ToListAsync();
        if (detailList == null)
        {
            return default;
        }

        var detail = detailList.Select(x => new MyFavoriteDetail()
        {
            Id = x.Id,
            MyFavoriteId = x.MyFavoriteId,
            Platform = x.Platform,
            MusicId = x.MusicId,
            MusicName = x.MusicName,
            MusicAlbum = x.MusicAlbum,
            MusicArtist = x.MusicArtist,
        }).ToList();
        return detail;
    }
}