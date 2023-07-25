using ListenTogether.Model;

namespace ListenTogether.Pages;
internal static class MusicModelConvertExtension
{
    public static List<LocalMusic> ToLocalMusics(this List<Music> musics)
    {
        return musics.Select(x => new LocalMusic()
        {
            Id = x.Id,
            Platform = (Model.Enums.PlatformEnum)x.Platform,
            IdOnPlatform = x.IdOnPlatform,
            Name = x.Name,
            Album = x.Album,
            Artist = x.Artist,
            ExtendDataJson = x.ExtendDataJson,
            ImageUrl = x.ImageUrl
        }).ToList();
    }

    public static LocalMusic ToLocalMusic(this Music music)
    {
        return new LocalMusic()
        {
            Id = music.Id,
            Platform = (Model.Enums.PlatformEnum)music.Platform,
            IdOnPlatform = music.IdOnPlatform,
            Name = music.Name,
            Album = music.Album,
            Artist = music.Artist,
            ExtendDataJson = music.ExtendDataJson,
            ImageUrl = music.ImageUrl
        };
    }
}