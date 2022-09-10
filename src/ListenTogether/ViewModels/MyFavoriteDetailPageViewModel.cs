using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
public partial class MyFavoriteDetailPageViewModel : ViewModelBase
{
    public int MyFavoriteId { get; set; }

    private readonly PlayerService _playerService;
    private readonly IServiceProvider _services;
    private readonly IPlaylistService _playlistService;
    private IMyFavoriteService _myFavoriteService;
    private IMusicService _musicService;
    public MyFavoriteDetailPageViewModel(IServiceProvider services, IPlaylistService playlistService, PlayerService playerService)
    {
        MyFavoriteMusics = new ObservableCollection<MyFavoriteDetailViewModel>();

        _services = services;
        _playerService = playerService;
        _playlistService = playlistService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (IsNotLogin)
            {
                await ToastService.Show("登录信息已过期，请重新登录");
                return;
            }

            StartLoading("");
            _myFavoriteService = _services.GetService<IMyFavoriteServiceFactory>().Create();
            _musicService = _services.GetService<IMusicServiceFactory>().Create();

            await LoadMyFavoriteInfoAsync();
            await GetMyFavoriteDetailAsync();

        }
        catch (Exception ex)
        {
            await ToastService.Show("歌单详情加载失败");
            Logger.Error("歌单详情页面初始化失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }

    [ObservableProperty]
    private MyFavoriteViewModel _currentMyFavorite;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMyFavoriteMusicsEmpty))]
    private ObservableCollection<MyFavoriteDetailViewModel> _myFavoriteMusics;
    public bool IsMyFavoriteMusicsEmpty => MyFavoriteMusics == null || MyFavoriteMusics.Count == 0;

    private async Task GetMyFavoriteDetailAsync()
    {
        MyFavoriteMusics.Clear();
        var myFavoriteDetailList = await _myFavoriteService.GetMyFavoriteDetailAsync(MyFavoriteId);
        int seq = 0;
        foreach (var myFavoriteDetail in myFavoriteDetailList)
        {
            MyFavoriteMusics.Add(new MyFavoriteDetailViewModel()
            {
                Seq = ++seq,
                Id = myFavoriteDetail.Id,
                PlatformName = myFavoriteDetail.PlatformName,
                MusicId = myFavoriteDetail.MusicId,
                MusicArtist = myFavoriteDetail.MusicArtist,
                MusicAlbum = myFavoriteDetail.MusicAlbum,
                MusicName = myFavoriteDetail.MusicName
            });
        }
    }

    private async Task LoadMyFavoriteInfoAsync()
    {
        var myFavorite = await _myFavoriteService.GetOneAsync(MyFavoriteId);
        CurrentMyFavorite = new MyFavoriteViewModel()
        {
            Id = myFavorite.Id,
            Name = myFavorite.Name,
            ImageUrl = myFavorite.ImageUrl,
            MusicCount = myFavorite.MusicCount
        };
    }

    [RelayCommand]
    private async void MyFavoriteRenameAsync()
    {
        string newName = await App.Current.MainPage.DisplayPromptAsync("歌单重命名", "请输入新的歌单名称：", "修改", "取消");
        if (newName.IsEmpty())
        {
            return;
        }

        if (CurrentMyFavorite.Name == newName)
        {
            return;
        }

        try
        {
            StartLoading("处理中....");

            var myFavorite = new MyFavorite()
            {
                Id = CurrentMyFavorite.Id,
                Name = newName,
                ImageUrl = CurrentMyFavorite.ImageUrl
            };

            await _myFavoriteService.AddOrUpdateAsync(myFavorite);

            await InitializeAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("重命名失败，网络出小差了");
            Logger.Error("歌单重命名失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }

    [RelayCommand]
    private async void MyFavoriteRemoveAsync()
    {
        var isOk = await Shell.Current.DisplayAlert("提示", "确定要删除该歌单吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }
        try
        {
            StartLoading("处理中....");
            var result = await _myFavoriteService.RemoveAsync(MyFavoriteId);
            if (result == false)
            {
                await ToastService.Show("删除失败");
                return;
            }
            await Shell.Current.GoToAsync($"..", true);
        }
        catch (Exception ex)
        {
            await ToastService.Show("删除失败，网络出小差了");
            Logger.Error("歌单删除失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }

    [RelayCommand]
    private async void PlayMusicAsync(MyFavoriteDetailViewModel selected)
    {
        var music = await _musicService.GetOneAsync(selected.MusicId);
        if (music == null)
        {
            await ToastService.Show("获取歌曲信息失败");
            return;
        }

        var playlist = new Playlist()
        {
            PlatformName = music.PlatformName,
            MusicId = music.Id,
            MusicName = music.Name,
            MusicArtist = music.Artist,
            MusicAlbum = music.Album
        };
        await _playlistService.AddToPlaylistAsync(playlist);
        await _playerService.PlayAsync(music);

        await Shell.Current.GoToAsync($"..", true);
    }

    [RelayCommand]
    private async void RemoveOneAsync(MyFavoriteDetailViewModel selected)
    {
        var isOk = await Shell.Current.DisplayAlert("提示", $"确定从歌单删除吗？{Environment.NewLine}{selected.MusicName}", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            StartLoading("处理中....");

            if (!await _myFavoriteService.RemoveDetailAsync(selected.Id))
            {
                await ToastService.Show("删除失败，网络出小差了");
                return;
            };
            await InitializeAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("删除失败，网络出小差了");
            Logger.Error("我的歌单删除歌曲失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }
}