using ListenTogether.Data.Entities;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Data.Repositories.Local;
public class MusicLocalRepository : IMusicRepository
{
    public async Task<Music?> GetOneAsync(string id)
    {
        var music = await DatabaseProvide.DatabaseAsync.Table<MusicEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (music == null)
        {
            return default;
        }

        return new Music()
        {
            Id = id,
            Platform = (PlatformEnum)music.Platform,
            PlatformInnerId = music.PlatformInnerId,
            //PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
            Name = music.Name,
            Album = music.Album,
            Artist = music.Artist,
            ImageUrl = music.ImageUrl,
            ExtendData = music.ExtendData,
        };
    }

    public async Task<bool> AddOrUpdateAsync(Music music)
    {
        var myMusic = await DatabaseProvide.DatabaseAsync.Table<MusicEntity>().FirstOrDefaultAsync(x => x.Id == music.Id);
        int count;
        if (myMusic == null)
        {
            myMusic = new MusicEntity()
            {
                Id = music.Id,
                Platform = (int)music.Platform,
                PlatformInnerId = music.PlatformInnerId,
                Name = music.Name,
                Album = music.Album,
                Artist = music.Artist,
                ImageUrl = music.ImageUrl,
                ExtendData = music.ExtendData
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(myMusic);
        }
        else
        {
            myMusic.Id = music.Id;
            myMusic.Platform = (int)music.Platform;
            myMusic.PlatformInnerId = music.PlatformInnerId;
            myMusic.Name = music.Name;
            myMusic.Album = music.Album;
            myMusic.Artist = music.Artist;
            myMusic.ImageUrl = music.ImageUrl;
            myMusic.ExtendData = music.ExtendData;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(myMusic);
        }
        if (count == 0)
        {
            return false;
        }

        return true;
    }
}
