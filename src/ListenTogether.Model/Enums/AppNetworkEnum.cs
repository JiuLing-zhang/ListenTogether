using System.ComponentModel;

namespace ListenTogether.Model.Enums;
public enum AppNetworkEnum
{
    [Description("单机版")]
    Standalone,
    [Description("网络版")]
    Online
}