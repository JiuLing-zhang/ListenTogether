namespace ListenTogether.Services;
public class UserFavoriteService
{
    /// <summary>
    /// 当前用户收藏的歌曲 id
    /// </summary>
    private static List<string> _musicsId = new List<string>();

    private readonly IMyFavoriteService _myFavoriteService;
    public UserFavoriteService(IMyFavoriteService myFavoriteService)
    {
        _myFavoriteService = myFavoriteService;
    }

    /// <summary>
    /// 加载当前用户收藏的歌曲 id
    /// </summary>
    public async Task LoadMusicsIdAsync()
    {
        _musicsId = await _myFavoriteService.GetAllMusicIdAsync();
    }

    /// <summary>
    /// 获取当前用户收藏的歌曲 id
    /// </summary>
    /// <returns></returns>
    public List<string> GetMusicsId()
    {
        return _musicsId;
    }
}