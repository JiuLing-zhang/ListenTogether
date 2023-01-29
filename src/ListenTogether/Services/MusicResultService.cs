using ListenTogether.Storage;

namespace ListenTogether.Services;
public class MusicResultService
{
    private readonly MusicPlayerService _musicPlayerService;
    private readonly IPlaylistService _playlistService;
    private readonly IMyFavoriteService _myFavoriteService;
    private readonly IMusicService _musicService;
    private readonly IMusicNetworkService _musicNetworkService;
    public MusicResultService(IPlaylistService playlistService, MusicPlayerService musicPlayerService, IMyFavoriteService myFavoriteService, IMusicService musicService, IMusicNetworkService musicNetworkService)
    {
        _playlistService = playlistService;
        _musicPlayerService = musicPlayerService;
        _myFavoriteService = myFavoriteService;
        _musicService = musicService;
        _musicNetworkService = musicNetworkService;
    }

    public async Task PlayAllAsync(List<LocalMusic> musics)
    {
        musics = await UpdateMusicDetail(musics);
        await AddToPlaylistAsync(musics);
        await PlayMusicAsync(musics.First());
    }

    public async Task PlayAsync(LocalMusic music)
    {
        music = await UpdateMusicDetail(music);
        await AddToPlaylistAsync(music);
        await PlayMusicAsync(music);
    }

    private async Task AddToPlaylistAsync(LocalMusic music)
    {
        var playlist = new Playlist()
        {
            Id = music.Id,
            IdOnPlatform = music.IdOnPlatform,
            Platform = music.Platform,
            Name = music.Name,
            Artist = music.Artist,
            Album = music.Album,
            ImageUrl = music.ImageUrl,
            ExtendDataJson = music.ExtendDataJson,
            EditTime = DateTime.Now
        };
        await _playlistService.AddToPlaylistAsync(playlist);
    }

    private async Task AddToPlaylistAsync(List<LocalMusic> musics)
    {
        var playlists = musics.Select(
            x => new Playlist()
            {
                Id = x.Id,
                IdOnPlatform = x.IdOnPlatform,
                Platform = x.Platform,
                Name = x.Name,
                Artist = x.Artist,
                Album = x.Album,
                ImageUrl = x.ImageUrl,
                ExtendDataJson = x.ExtendDataJson,
                EditTime = DateTime.Now
            }).ToList();

        await _playlistService.AddToPlaylistAsync(playlists);
    }

    private async Task PlayMusicAsync(LocalMusic music)
    {
        await _musicPlayerService.PlayAsync(music.Id);
    }

    public async Task AddToFavoriteAsync(LocalMusic music)
    {
        if (UserInfoStorage.GetUsername().IsEmpty())
        {
            await ToastService.Show("用户未登录");
            return;
        }

        music = await UpdateMusicDetail(music);
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

            var isMusicOk = await _musicService.AddOrUpdateAsync(music);
            if (!isMusicOk)
            {
                await ToastService.Show("歌曲信息保存失败");
                return;
            }

            var result = await _myFavoriteService.AddMusicToMyFavoriteAsync(selectedMyFavoriteId, music.Id);
            if (result == false)
            {
                await ToastService.Show("添加失败");
                return;
            }
            if (GlobalConfig.MyUserSetting.Play.IsPlayWhenAddToFavorite)
            {
                await PlayMusicAsync(music);
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

    /// <summary>
    /// 更新歌曲信息
    /// </summary>
    private async Task<LocalMusic> UpdateMusicDetail(LocalMusic music)
    {
        var myMusic = music;
        if (myMusic.ImageUrl.IsEmpty() && myMusic.Platform == Model.Enums.PlatformEnum.KuGou)
        {
            myMusic.ImageUrl = await _musicNetworkService.GetImageUrlAsync(myMusic.Platform, myMusic.IdOnPlatform, myMusic.ExtendDataJson);
        }
        return myMusic;
    }

    /// <summary>
    /// 更新歌曲信息
    /// </summary>
    private async Task<List<LocalMusic>> UpdateMusicDetail(List<LocalMusic> musics)
    {
        var myMusics = musics;
        for (int i = 0; i < myMusics.Count; i++)
        {
            myMusics[i] = await UpdateMusicDetail(myMusics[i]);
        }
        return myMusics;
    }
}