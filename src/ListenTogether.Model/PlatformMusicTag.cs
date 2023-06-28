namespace ListenTogether.Model;
public class PlatformMusicTag
{
    public List<MusicTag> HotTags { get; set; }

    public List<MusicTypeTag> AllTypes { get; set; }

    public PlatformMusicTag(List<MusicTag> hotTags, List<MusicTypeTag> allTypes)
    {
        HotTags = hotTags;
        AllTypes = allTypes;
    }
}