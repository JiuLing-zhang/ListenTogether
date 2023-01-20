using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
public class DiscoverTag
{
    public string CurrentTag { get; set; }
    public List<MusicTag> HotTags { get; set; }
    public List<MusicTypeTag> AllTypes { get; set; }
    public DiscoverTag(string currentTag, List<MusicTag> hotTags, List<MusicTypeTag> allTypes)
    {
        HotTags = hotTags;
        AllTypes = allTypes;
        CurrentTag = currentTag;
    }
}