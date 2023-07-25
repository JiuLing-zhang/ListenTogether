using NetMusicLib.Models;

namespace ListenTogether.Filters.MusicSearchFilter;
internal class VipMusicFilter : IMusicSearchFilter
{
    public List<Music> Filter(List<Music> musics)
    {
        return musics.Where(x => x.Fee != (NetMusicLib.Enums.FeeEnum)Model.Enums.FeeEnum.Vip).ToList();
    }
}