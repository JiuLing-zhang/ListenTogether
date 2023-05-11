using ListenTogether.Pages;

namespace ListenTogetherMauiBlazor;
public class WifiOptionsService : IWifiOptionsService
{
    public async Task<bool> HasWifiOrCanPlayAsync()
    {
        if (Config.Desktop)
        {
            return true;
        }

        var current = Connectivity.NetworkAccess;

        if (current != NetworkAccess.Internet)
        {
            return false;
        }

        var profiles = Connectivity.ConnectionProfiles;
        if (profiles.Contains(ConnectionProfile.WiFi))
        {
            return true;
        }

        return await Shell.Current.DisplayAlert("提示", "当前为非WIFI环境，确定用流量播放吗？", "允许本次", "取消");
    }
}