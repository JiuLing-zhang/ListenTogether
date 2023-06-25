namespace ListenTogether.Data;
internal class ApiSettings
{
    public string DeviceId { get; set; }
    public ApiSettings(string deviceId)
    {
        DeviceId = deviceId;
    }
    public UserUrl User => new(DeviceId);
    public class UserUrl
    {
        private readonly string _deviceId;
        public UserUrl(string deviceId)
        {
            _deviceId = deviceId;
        }
        public string Register => $"/api/user/reg";
        public string Edit => $"/api/user/edit";
        public string Login => $"/api/user/{_deviceId}/login";
        public string RefreshToken => $"/api/user/{_deviceId}/refresh-token";
        public string Logout => $"/api/user/{_deviceId}/logout";
    }

    public UserConfigUrl UserConfig => new();
    public class UserConfigUrl
    {
        public string Get => $"/api/user-config";
        public string WriteGeneral => $"/api/user-config/general";
        public string WritePlayConfig => $"/api/user-config/play";
        public string WriteSearchConfig => $"/api/user-config/search";
    }

    public PlaylistUrl Playlist => new();
    public class PlaylistUrl
    {
        public string GetAll => $"/api/playlist/list";
        public string AddOrUpdate => $"/api/playlist";
        public string Remove => $"/api/playlist/delete/{{0}}";
        public string RemoveAll => $"/api/playlist/clear";
    }

    public MyFavoriteUrl MyFavorite => new();
    public class MyFavoriteUrl
    {
        public string Get => $"/api/my-favorite/{{0}}";
        public string GetAll => $"/api/my-favorite/list";
        public string GetDetail => $"/api/my-favorite/detail/{{0}}";
        public string NameExist => $"/api/my-favorite/name-exist/{{0}}";
        public string AddOrUpdate => $"/api/my-favorite";
        public string Remove => $"/api/my-favorite/delete/{{0}}";
        public string AddMusic => $"/api/my-favorite/add-music/{{0}}/{{1}}";
        public string RemoveDetail => $"/api/my-favorite/remove-detail/{{0}}";
    }

    public MusicUrl Music => new();
    public class MusicUrl
    {
        public MusicUrl()
        {
        }
        public string Get => $"/api/music/{{0}}";
        public string AddOrUpdate => $"/api/music";
    }

    public string WriteLog => $"/api/log/write-all";
}