namespace MusicPlayerOnline.Data;
internal class ApiSettings
{
    public ApiSettings(string urlBase, string deviceId)
    {
        BaseUrl = urlBase;
        DeviceId = deviceId;
    }
    public string BaseUrl { get; set; }
    public string DeviceId { get; set; }

    public UserUrl User => new(BaseUrl, DeviceId);
    public class UserUrl
    {
        private readonly string _baseUrl;
        private readonly string _deviceId;
        public UserUrl(string urlBase, string deviceId)
        {
            _baseUrl = urlBase;
            _deviceId = deviceId;
        }
        public string Register => $"{_baseUrl}/api/user/reg";
        public string Login => $"{_baseUrl}/api/user/{_deviceId}/login";
        public string RefreshToken => $"{_baseUrl}/api/user/{_deviceId}/refresh-token";
        public string Logout => $"{_baseUrl}/api/user/{_deviceId}/logout";
    }

    public UserConfigUrl UserConfig => new(BaseUrl);
    public class UserConfigUrl
    {
        private readonly string _baseUrl;
        public UserConfigUrl(string urlBase)
        {
            _baseUrl = urlBase;
        }
        public string Get => $"{_baseUrl}/api/user-config";
        public string WriteGeneral => $"{_baseUrl}/api/user-config/general";
        public string WritePlayConfig => $"{_baseUrl}/api/user-config/play";
        public string WriteSearchConfig => $"{_baseUrl}/api/user-config/search";
    }

    public PlaylistUrl Playlist => new(BaseUrl);
    public class PlaylistUrl
    {
        private readonly string _baseUrl;
        public PlaylistUrl(string urlBase)
        {
            _baseUrl = urlBase;
        }
        public string GetAll => $"{_baseUrl}/api/playlist/list";
        public string AddOrUpdate => $"{_baseUrl}/api/playlist";
        public string Remove => $"{_baseUrl}/api/playlist/delete/{{0}}";
        public string RemoveAll => $"{_baseUrl}/api/playlist/clear";
    }

    public MyFavoriteUrl MyFavorite => new(BaseUrl);
    public class MyFavoriteUrl
    {
        private readonly string _baseUrl;
        public MyFavoriteUrl(string urlBase)
        {
            _baseUrl = urlBase;
        }
        public string Get => $"{_baseUrl}/api/my-favorite/{{0}}";
        public string GetAll => $"{_baseUrl}/api/my-favorite/list";
        public string GetDetail => $"{_baseUrl}/api/my-favorite/detail/{{0}}";
        public string AddOrUpdate => $"{_baseUrl}/api/my-favorite";
        public string Remove => $"{_baseUrl}/api/my-favorite/delete/{{0}}";
        public string AddMusic => $"{_baseUrl}/api/my-favorite/add-music/{{0}}";
    }

    public MusicUrl Music => new(BaseUrl);
    public class MusicUrl
    {
        private readonly string _baseUrl;
        public MusicUrl(string urlBase)
        {
            _baseUrl = urlBase;
        }
        public string Get => $"{_baseUrl}/api/music/{{0}}";
        public string AddOrUpdate => $"{_baseUrl}/api/music";
        public string UpdateCache => $"{_baseUrl}/api/music/update-cache/{{0}}/{{1}}";
    }
}