using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data.Entities;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Data.Repositories.Local;
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
            PlatformId = music.PlatformId,
            PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
            Name = music.Name,
            Album = music.Album,
            Alias = music.Alias,
            Artist = music.Artist,
            CachePath = music.CachePath,
            Duration = music.Duration,
            ImageUrl = music.ImageUrl,
            Lyric = music.Lyric,
            PlayUrl = music.PlayUrl
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
                PlatformId = music.PlatformId,
                Name = music.Name,
                Album = music.Album,
                Alias = music.Alias,
                Artist = music.Artist,
                CachePath = music.CachePath,
                Duration = music.Duration,
                ImageUrl = music.ImageUrl,
                Lyric = music.Lyric,
                PlayUrl = music.PlayUrl
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(myMusic);
        }
        else
        {
            myMusic.Id = music.Id;
            myMusic.Platform = (int)music.Platform;
            myMusic.PlatformId = music.PlatformId;
            myMusic.Name = music.Name;
            myMusic.Album = music.Album;
            myMusic.Alias = music.Alias;
            myMusic.Artist = music.Artist;
            myMusic.CachePath = music.CachePath;
            myMusic.Duration = music.Duration;
            myMusic.ImageUrl = music.ImageUrl;
            myMusic.Lyric = music.Lyric;
            myMusic.PlayUrl = music.PlayUrl;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(myMusic);
        }
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateCacheAsync(string id, string cachePath)
    {
        var music = await DatabaseProvide.DatabaseAsync.Table<MusicEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (music == null)
        {
            return false;
        }

        //缓存路径没有改变时不更新
        if (music.CachePath == cachePath)
        {
            return true;
        }

        music.CachePath = cachePath;
        var count = await DatabaseProvide.DatabaseAsync.UpdateAsync(music);
        if (count == 0)
        {
            return false;
        }

        return true;
    }
}
