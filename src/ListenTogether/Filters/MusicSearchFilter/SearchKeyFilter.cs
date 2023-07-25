using NetMusicLib.Models;

namespace ListenTogether.Filters.MusicSearchFilter;
internal class SearchKeyFilter : IMusicSearchFilter
{
    private string _searchKey;
    public SearchKeyFilter(string searchKey)
    {
        _searchKey = searchKey;
    }
    public List<Music> Filter(List<Music> musics)
    {
        return musics.Where(x => x.Name.IndexOf(_searchKey) >= 0 || x.Artist.IndexOf(_searchKey) >= 0).ToList();
    }
}