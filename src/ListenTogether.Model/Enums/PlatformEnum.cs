using System.ComponentModel;

namespace ListenTogether.Model.Enums
{
    [Flags]
    public enum PlatformEnum
    {
        [Description("网易")]
        NetEase = 1,
        [Description("酷狗")]
        KuGou = 2,
        [Description("咪咕")]
        MiGu = 4,
        [Description("酷我")]
        KuWo = 8,
    }
}
