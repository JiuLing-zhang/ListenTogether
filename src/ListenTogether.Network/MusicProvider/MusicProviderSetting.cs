using ListenTogether.Model.Enums;

namespace ListenTogether.Network.MusicProvider;
internal class MusicProviderSetting
{
    private MusicProviderSetting()
    {

    }

    public static MusicFormatTypeEnum MusicFormatType { get; set; }
}