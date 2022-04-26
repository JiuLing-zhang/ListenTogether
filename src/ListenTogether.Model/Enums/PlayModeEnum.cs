using System.ComponentModel;

namespace ListenTogether.Model.Enums
{
    public enum PlayModeEnum
    {
        [Description("单曲循环")]
        RepeatOne,
        [Description("列表循环")]
        RepeatList,
        [Description("随机播放")]
        Shuffle
    }
}
