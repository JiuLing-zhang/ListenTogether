using NetMusicLib.Models;

namespace ListenTogether.Filters.MusicSearchFilter;

internal interface IMusicSearchFilter
{
    public List<Music> Filter(List<Music> musics);
}