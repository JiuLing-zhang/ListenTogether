using System.ComponentModel;

namespace ListenTogether.Model.Enums
{
    [Flags]
    public enum PlatformEnum
    {
        [Description("网易云音乐")]
        NetEase = 1,
        [Description("酷狗音乐")]
        KuGou = 2,
        [Description("咪咕音乐")]
        MiGu = 4,
        [Description("酷我音乐")]
        KuWo = 8,
    }
}
