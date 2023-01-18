namespace ListenTogether.Filters.MusicSearchFilter;

internal interface IMusicSearchFilter
{
    public List<MusicResultShow> Filter(List<MusicResultShow> musics);
}