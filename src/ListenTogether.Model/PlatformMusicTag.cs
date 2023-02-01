using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
public class PlatformMusicTag
{
    public PlatformEnum Platform { get; set; }

    public List<MusicTag> HotTags { get; set; }

    public List<MusicTypeTag> AllTypes { get; set; }

    public PlatformMusicTag(PlatformEnum platform, List<MusicTag> hotTags, List<MusicTypeTag> allTypes)
    {
        Platform = platform;
        HotTags = hotTags;
        AllTypes = allTypes;
    }
}