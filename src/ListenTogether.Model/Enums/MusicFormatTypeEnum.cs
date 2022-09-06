using System.ComponentModel;

namespace ListenTogether.Model.Enums;
public enum MusicFormatTypeEnum
{
    [Description("普通")]
    PQ = 1,

    [Description("高品质")]
    HQ = 3,

    [Description("超高品质")]
    SQ = 5,

    [Description("比超高还高，应该叫啥？")]
    ZQ = 7
}
