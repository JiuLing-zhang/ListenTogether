using ListenTogether.Model;

namespace ListenTogether.Filters.MusicSearchFilter;
internal class ShortMusicFilter : IMusicSearchFilter
{
    public List<MusicResultShow> Filter(List<MusicResultShow> musics)
    {
        return musics.Where(x => x.Duration.TotalMilliseconds == 0 || x.Duration.TotalMilliseconds >= 60 * 1000).ToList();
    }
}