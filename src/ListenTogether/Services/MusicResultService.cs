namespace ListenTogether.Services;
public class MusicResultService
{
    private readonly MusicPlayerService _musicPlayerService;
    private readonly IServiceProvider _services;
    private readonly IPlaylistService _playlistService;
    private IMyFavoriteService _myFavoriteService;
    private IMusicService _musicService;
    public MusicResultService(IServiceProvider services, IPlaylistService playlistService, MusicPlayerService musicPlayerService)
    {
        _services = services;
        _playlistService = playlistService;
        _musicPlayerService = musicPlayerService;
    }

    public async Task PlayAllAsync(List<MusicResultShowViewModel> musicResultList)
    {
        await AddToPlaylistAsync(musicResultList);
        await PlayMusicAsync(musicResultList.First());
    }

    public async Task PlayAsync(MusicResultShowViewModel musicResult)
    {
        await AddToPlaylistAsync(musicResult);
        await PlayMusicAsync(musicResult);
    }

    private async Task AddToPlaylistAsync(MusicResultShowViewModel musicResult)
    {
        var playlist = new Playlist()
        {
            MusicId = musicResult.Id,
            MusicIdOnPlatform = musicResult.IdOnPlatform,
            Platform = musicResult.Platform,
            MusicName = musicResult.Name,
            MusicArtist = musicResult.Artist,
            MusicAlbum = musicResult.Album,
            MusicImageUrl = musicResult.ImageUrl,
            EditTime = DateTime.Now
        };
        await _playlistService.AddToPlaylistAsync(playlist);
    }

    private async Task AddToPlaylistAsync(List<MusicResultShowViewModel> musicResultList)
    {
        var playlists = musicResultList.Select(
            x => new Playlist()
            {
                MusicId = x.Id,
                MusicIdOnPlatform = x.IdOnPlatform,
                Platform = x.Platform,
                MusicName = x.Name,
                MusicArtist = x.Artist,
                MusicAlbum = x.Album,
                MusicImageUrl = x.ImageUrl,
                EditTime = DateTime.Now
            }).ToList();

        await _playlistService.AddToPlaylistAsync(playlists);
    }

    private async Task PlayMusicAsync(MusicResultShowViewModel musicResult)
    {
        await _musicPlayerService.PlayAsync(musicResult.Id);
    }

    public async Task AddToFavoriteAsync(MusicResultShowViewModel musicResult)
    {
        _myFavoriteService = _services.GetRequiredService<IMyFavoriteServiceFactory>().Create();
        _musicService = _services.GetRequiredService<IMusicServiceFactory>().Create();

        var music = new Music()
        {
            Id = musicResult.Id,
            Platform = musicResult.Platform,
            PlatformInnerId = musicResult.IdOnPlatform,
            Album = musicResult.Album,
            Artist = musicResult.Artist,
            ImageUrl = musicResult.ImageUrl,
            Name = musicResult.Name,
            ExtendData = musicResult.ExtendDataJson
        };
        var isMusicOk = await _musicService.AddOrUpdateAsync(music);

        if (!isMusicOk)
        {
            await ToastService.Show("歌曲信息保存失败");
            return;
        }
        try
        {
            //构造待选择的歌单项
            string[]? myFavoriteButtons = null;
            var myFavoriteList = await _myFavoriteService.GetAllAsync();
            if (myFavoriteList != null)
            {
                myFavoriteButtons = myFavoriteList.Select(x => x.Name).ToArray();
            }

            int selectedMyFavoriteId;
            string myFavoriteItem = await App.Current.MainPage.DisplayActionSheet("请选择要加入的歌单", "取消", "创建一个新歌单", myFavoriteButtons);
            if (myFavoriteItem.IsEmpty() || myFavoriteItem == "取消")
            {
                return;
            }

            if (myFavoriteItem != "创建一个新歌单" && myFavoriteList != null)
            {
                //使用已有歌单
                selectedMyFavoriteId = myFavoriteList.First(x => x.Name == myFavoriteItem).Id;
            }
            else
            {
                //新增歌单
                string myFavoriteName = await App.Current.MainPage.DisplayPromptAsync("添加歌单", "请输入歌单名称：", "添加", "取消");
                if (myFavoriteName.IsEmpty())
                {
                    await ToastService.Show("操作取消");
                    return;
                }

                if (await _myFavoriteService.NameExistAsync(myFavoriteName))
                {
                    await ToastService.Show("歌单名称已存在");
                    return;
                }

                var myFavorite = new MyFavorite()
                {
                    Name = myFavoriteName,
                    MusicCount = 0
                };
                var newMyFavorite = await _myFavoriteService.AddOrUpdateAsync(myFavorite);
                if (newMyFavorite == null)
                {
                    await ToastService.Show("添加失败");
                    return;
                }
                selectedMyFavoriteId = newMyFavorite.Id;
            }

            var result = await _myFavoriteService.AddMusicToMyFavoriteAsync(selectedMyFavoriteId, music);
            if (result == false)
            {
                await ToastService.Show("添加失败");
                return;
            }
            if (GlobalConfig.MyUserSetting.Play.IsPlayWhenAddToFavorite)
            {
                await PlayMusicAsync(musicResult);
            }
            else
            {
                await ToastService.Show("添加成功");
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("添加失败，网络出小差了");
            Logger.Error("搜索结果添加到我的歌单失败。", ex);
        }
    }
}