using ListenTogether.Data.Entities;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Data.Repositories.Local;
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
            MusicCount = musicCount,
            EditTime = myFavorite.EditTime
        };
    }

    public async Task<List<MyFavorite>?> GetAllAsync()
    {
        string sql = $"SELECT Id,Name,ImageUrl,EditTime,(SELECT COUNT(*) FROM MyFavoriteDetail mfd WHERE MyFavoriteId=mf.Id)AS MusicCount FROM MyFavorite mf";
        var myFavorites = await DatabaseProvide.DatabaseAsync.QueryAsync<MyFavoriteEntity>(sql);
        return myFavorites.Select(x => new MyFavorite()
        {
            Id = x.Id,
            ImageUrl = x.ImageUrl,
            MusicCount = x.MusicCount,
            Name = x.Name,
            EditTime = x.EditTime
        }).ToList();
    }

    public async Task<bool> NameExist(string myFavoriteName)
    {
        var count = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteEntity>().CountAsync(x => x.Name == myFavoriteName);
        if (count > 0)
        {
            return true;
        }
        return false;
    }

    public async Task<MyFavorite?> AddOrUpdateAsync(MyFavorite myFavorite)
    {
        var editTime = DateTime.Now;
        var favorite = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteEntity>().FirstOrDefaultAsync(x => x.Id == myFavorite.Id);
        int count;
        if (favorite == null)
        {
            favorite = new MyFavoriteEntity()
            {
                Name = myFavorite.Name,
                ImageUrl = myFavorite.ImageUrl,
                EditTime = editTime
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(favorite);
        }
        else
        {
            favorite.Name = myFavorite.Name;
            favorite.ImageUrl = myFavorite.ImageUrl;
            favorite.EditTime = editTime;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(favorite);
        }
        if (count == 0)
        {
            return default;
        }

        return new MyFavorite()
        {
            Id = favorite.Id,
            Name = favorite.Name,
            ImageUrl = favorite.ImageUrl,
            EditTime = editTime
        };
    }

    public async Task<bool> RemoveAsync(int id)
    {
        string sql = $"DELETE FROM MyFavoriteDetail WHERE MyFavoriteId={id}";
        await DatabaseProvide.DatabaseAsync.ExecuteAsync(sql);
        await DatabaseProvide.DatabaseAsync.DeleteAsync<MyFavoriteEntity>(id);
        return true;
    }
    public async Task<bool> AddMusicToMyFavorite(int id, Music music)
    {
        var favorite = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (favorite == null)
        {
            return false;
        }

        var favoriteDetail = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetailEntity>().FirstOrDefaultAsync(x => x.MyFavoriteId == id && x.MusicId == music.Id);
        try
        {
            await DatabaseProvide.DatabaseAsync.RunInTransactionAsync(tran =>
               {
                   if (favoriteDetail == null)
                   {
                       favoriteDetail = new MyFavoriteDetailEntity()
                       {
                           PlatformName = music.PlatformName,
                           MyFavoriteId = id,
                           MusicName = music.Name,
                           MusicId = music.Id,
                           MusicAlbum = music.Album,
                           MusicArtist = music.Artist
                       };
                       tran.Insert(favoriteDetail);
                   }
                   else
                   {
                       favoriteDetail.MusicId = music.Id;
                       favoriteDetail.MusicName = music.Name;
                       favoriteDetail.MusicArtist = music.Artist;
                       favoriteDetail.MusicAlbum = music.Album;
                       tran.Update(favoriteDetail);
                   }

                   //更新歌单图标，歌单可以前置添加，所以有可能会没有图标
                   if (favorite.ImageUrl.IsEmpty() && music.ImageUrl.IsNotEmpty())
                   {
                       favorite.ImageUrl = music.ImageUrl;
                   }
                   favorite.EditTime = DateTime.Now;
                   tran.Update(favorite);
               });
        }
        catch (Exception)
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
            PlatformName = x.PlatformName,
            MusicId = x.MusicId,
            MusicName = x.MusicName,
            MusicAlbum = x.MusicAlbum,
            MusicArtist = x.MusicArtist,
        }).ToList();
        return detail;
    }

    public async Task<bool> RemoveDetailAsync(int id)
    {
        await DatabaseProvide.DatabaseAsync.RunInTransactionAsync(async tran =>
        {
            var detail = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteDetailEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (detail == null)
            {
                return;
            }

            tran.Delete<MyFavoriteDetailEntity>(id);
            var favorite = await DatabaseProvide.DatabaseAsync.Table<MyFavoriteEntity>().FirstOrDefaultAsync(x => x.Id == detail.MyFavoriteId);
            favorite.EditTime = DateTime.Now;
            tran.Update(favorite);
        });
        return true;
    }
}