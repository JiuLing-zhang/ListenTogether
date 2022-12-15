namespace ListenTogether.Filters.MusicSearchFilter;
internal class SearchKeyFilter : IMusicSearchFilter
{
        private string _searchKey;
    public SearchKeyFilter(string searchKey)
    {
        _searchKey = searchKey;
    }
    public List<MusicSearchResult> Filter(List<MusicSearchResult> musics)
    {
        return musics.Where(x => x.Name.IndexOf(_searchKey) >= 0 || x.Artist.IndexOf(_searchKey) >= 0).ToList();
    }
}