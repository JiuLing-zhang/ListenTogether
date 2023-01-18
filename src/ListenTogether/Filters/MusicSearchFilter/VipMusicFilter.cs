namespace ListenTogether.Filters.MusicSearchFilter;
internal class VipMusicFilter : IMusicSearchFilter
{
    public List<MusicResultShow> Filter(List<MusicResultShow> musics)
    {
        return musics.Where(x => x.Fee != Model.Enums.FeeEnum.Vip).ToList();
    }
}