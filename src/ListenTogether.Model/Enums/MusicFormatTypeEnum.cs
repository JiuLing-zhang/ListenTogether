using System.ComponentModel;

namespace ListenTogether.Model.Enums;
public enum MusicFormatTypeEnum
{
    [Description("普通")]
    PQ = 1,

    [Description("高级")]
    HQ = 3,

    [Description("超高")]
    SQ = 5,

    [Description("极致")]
    ZQ = 7
}
