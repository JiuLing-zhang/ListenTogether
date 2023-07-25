using NetMusicLib.Models;

namespace ListenTogether.Filters.MusicSearchFilter;
internal class ShortMusicFilter : IMusicSearchFilter
{
    public List<Music> Filter(List<Music> musics)
    {
        return musics.Where(x => x.Duration.TotalMilliseconds == 0 || x.Duration.TotalMilliseconds >= 60 * 1000).ToList();
    }
}