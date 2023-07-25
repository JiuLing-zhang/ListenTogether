using NetMusicLib.Enums;

namespace ListenTogether.Filters.MusicSearchFilter;
internal class VipMusicFilter : IMusicSearchFilter
{
    public List<Music> Filter(List<Music> musics)
    {
        return musics.Where(x => x.Fee != FeeEnum.Vip).ToList();
    }
}