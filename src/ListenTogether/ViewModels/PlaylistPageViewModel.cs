using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class PlaylistPageViewModel : ViewModelBase
{
    private readonly IMusicService _musicService;
    private readonly IMyFavoriteService _myFavoriteService;
    private readonly IPlaylistService _playlistService;
    private readonly MusicPlayerService _playerService;

    public PlaylistPageViewModel(IPlaylistService playlistService, MusicPlayerService playerService, IMusicService musicService, IMyFavoriteService myFavoriteService)
    {
        Playlist = new ObservableCollection<PlaylistViewModel>();

        _playerService = playerService;
        _playlistService = playlistService;
        _musicService = musicService;
        _myFavoriteService = myFavoriteService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            StartLoading("页面加载中....");
            await GetPlaylistAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("播放列表加载失败");
            Logger.Error("播放列表页面初始化失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }
    private async Task GetPlaylistAsync()
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist == null || playlist.Count == 0)
        {
            if (Playlist.Count > 0)
            {
                Playlist.Clear();
            }
            IsPlaylistEmpty = !Playlist.Any();
            return;
        }

        if (playlist.Count == Playlist.Count)
        {
            //数据未发生变更时不更新列表
            var dbLastEditTime = playlist.OrderByDescending(x => x.EditTime).First().EditTime;
            var pageLast = Playlist.OrderByDescending(x => x.EditTime).FirstOrDefault();

            if (pageLast != null && pageLast.EditTime.Subtract(dbLastEditTime).TotalDays >= 0)
            {
                return;
            }
        }

        if (Playlist.Count > 0)
        {
            Playlist.Clear();
        }
        foreach (var item in playlist)
        {
            Playlist.Add(new PlaylistViewModel()
            {
                PlatformName = item.Platform.GetDescription(),
                MusicId = item.Id,
                MusicName = item.Name,
                MusicArtist = item.Artist,
                MusicAlbum = item.Album,
                EditTime = item.EditTime
            });
        }

        IsPlaylistEmpty = !Playlist.Any();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPlaylistNotEmpty))]
    private bool _isPlaylistEmpty;
    public bool IsPlaylistNotEmpty => !IsPlaylistEmpty;

    /// <summary>
    /// 播放列表
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<PlaylistViewModel> _playlist = null!;

    [RelayCommand]
    private async void PlayMusicAsync(PlaylistViewModel selected)
    {
        var music = await _musicService.GetOneAsync(selected.MusicId);
        if (music == null)
        {
            await ToastService.Show("获取歌曲信息失败");
            return;
        }

        await _playerService.PlayAsync(music.Id);
    }

    [RelayCommand]
    private async void AddToMyFavoriteAsync(PlaylistViewModel selected)
    {
        try
        {
            StartLoading("处理中....");

            var music = await _musicService.GetOneAsync(selected.MusicId);
            if (music == null)
            {
                await ToastService.Show("歌曲不存在");
                return;
            }

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
                await _playerService.PlayAsync(music.Id);
            }
            else
            {
                await ToastService.Show("添加成功");
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("添加失败，网络出小差了");
            Logger.Error("播放列表添加到我的歌单失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }

    [RelayCommand]
    private async void RemoveOneAsync(PlaylistViewModel selected)
    {
        try
        {
            StartLoading("正在删除....");
            if (!await _playlistService.RemoveAsync(selected.Id))
            {
                await ToastService.Show("删除失败");
                return;
            }
            await GetPlaylistAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("删除失败，网络出小差了");
            Logger.Error("播放列表删除歌曲失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }

    [RelayCommand]
    private async void ClearPlaylistAsync()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除播放列表吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }
        if (!await _playlistService.RemoveAllAsync())
        {
            await ToastService.Show("删除失败");
            return;
        }
        await InitializeAsync();
    }
}
