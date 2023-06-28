using ListenTogether.Model.Enums;

namespace ListenTogether.Model;
//TODO 重命名，并且修改标签接口返回值类型
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