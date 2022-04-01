namespace MusicPlayerOnline.Maui.Services;

public class WifiOptionsService
{
    public bool HasWifiOrCanPlayWithOutWifi()
    {
        if (Config.Desktop)
        {
            return true;
        }

        var canPlayMusic = false;
        var current = Connectivity.NetworkAccess;

        if (current != NetworkAccess.Internet)
        {
            ToastService.Show("播放失败：网络不可用");
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
                ToastService.Show("播放失败：当前为非WIFI环境");
            }
        }
        return canPlayMusic;
    }
}
