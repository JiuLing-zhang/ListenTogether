namespace ListenTogether.Extensions;
internal static class MusicModelConvertExtension
{
    public static List<LocalMusic> ToLocalMusics(this List<MusicResultShowViewModel> musics)
    {
        return musics.Select(x => new LocalMusic()
        {
            Id = x.Id,
            Platform = x.Platform,
            IdOnPlatform = x.IdOnPlatform,
            Name = x.Name,
            Album = x.Album,
            Artist = x.Artist,
            ExtendDataJson = x.ExtendDataJson,
            ImageUrl = x.ImageUrl
        }).ToList();
    }

    public static LocalMusic ToLocalMusic(this MusicResultShowViewModel music)
    {
        return new LocalMusic()
        {
            Id = music.Id,
            Platform = music.Platform,
            IdOnPlatform = music.IdOnPlatform,
            Name = music.Name,
            Album = music.Album,
            Artist = music.Artist,
            ExtendDataJson = music.ExtendDataJson,
            ImageUrl = music.ImageUrl
        };
    }
}