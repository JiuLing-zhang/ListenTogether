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
            PlatformInnerId = music.PlatformInnerId,
            PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
            Name = music.Name,
            Album = music.Album,
            Alias = music.Alias,
            Artist = music.Artist,
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
                PlatformInnerId = music.PlatformInnerId,
                Name = music.Name,
                Album = music.Album,
                Alias = music.Alias,
                Artist = music.Artist,
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
            myMusic.PlatformInnerId = music.PlatformInnerId;
            myMusic.Name = music.Name;
            myMusic.Album = music.Album;
            myMusic.Alias = music.Alias;
            myMusic.Artist = music.Artist;
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
}
