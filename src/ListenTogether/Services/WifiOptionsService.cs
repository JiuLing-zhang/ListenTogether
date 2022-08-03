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
            await ToastService.Show("播放失败：网络不可用");
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
                await ToastService.Show("播放失败：当前为非WIFI环境");
            }
        }
        return canPlayMusic;
    }
}
