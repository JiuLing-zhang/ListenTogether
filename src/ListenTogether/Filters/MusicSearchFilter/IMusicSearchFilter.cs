namespace ListenTogether.Filters.MusicSearchFilter;

internal interface IMusicSearchFilter
{
    public List<MusicSearchResult> Filter(List<MusicSearchResult> musics);
}