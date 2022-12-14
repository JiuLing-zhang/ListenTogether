namespace ListenTogether.Filters.MusicSearchFilter;
//test
internal interface IMusicSearchFilter
{
    public List<MusicSearchResult> Filter(List<MusicSearchResult> musics);
}