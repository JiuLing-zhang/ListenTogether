using System.ComponentModel;

namespace MusicPlayerOnline.Model.Enums
{
    [Flags]
    public enum PlatformEnum
    {
        [Description("网易")]
        Netease = 1,
        [Description("酷狗")]
        KuGou = 2,
        [Description("咪咕")]
        MiGu = 4
    }
}
