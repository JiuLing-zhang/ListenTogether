namespace ListenTogether.Services;

public class WifiOptionsService
{
    public async Task<bool> HasWifiOrCanPlayWithOutWifiAsync()
    {
        if (Config.Desktop)
        {
            return true;
        }

        var canPlayMusic = false;
        var current = Connectivity.NetworkAccess;

        if (current != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("错误", "播放失败：网络不可用", "关闭");
        }
        else
        {
            var profiles = Connectivity.ConnectionProfiles;
            var hasWifi = profiles.Contains(ConnectionProfile.WiFi);

            if (!GlobalConfig.MyUserSetting.Play.IsWifiPlayOnly || hasWifi)
            {
                canPlayMusic = true;
            }
            else
            {
                canPlayMusic = await Shell.Current.DisplayAlert("提示", "当前为非WIFI环境，确定用流量播放吗？", "允许本次", "取消");
            }
        }
        return canPlayMusic;
    }
}
