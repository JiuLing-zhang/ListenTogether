using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
public class MyFavoriteDetailPageViewModel : ViewModelBase
{
    public int MyFavoriteId { get; set; }

    private IServiceProvider _services;
    private IMyFavoriteService _myFavoriteService;
    private IPlaylistService _playlistService;
    private IMusicService _musicService;
    private PlayerService _playerService;
    public ICommand PlayMusicCommand => new Command<MyFavoriteDetailViewModel>(PlayMusic);
    public ICommand MyFavoriteRenameCommand => new Command(RenameMyFavorite);
    public ICommand MyFavoriteRemoveCommand => new Command(MyFavoriteRemove);
    public ICommand RemoveOneCommand => new Command<MyFavoriteDetailViewModel>(RemoveOne);

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
            IsBusy = true;
            _myFavoriteService = _services.GetService<IMyFavoriteServiceFactory>().Create();
            _musicService = _services.GetService<IMusicServiceFactory>().Create();

            await LoadMyFavoriteInfo();
            await GetMyFavoriteDetail();

            OnPropertyChanged("IsMyFavoriteMusicsEmpty");
        }
        catch (Exception ex)
        {
            await ToastService.Show("歌单详情加载失败");
            Logger.Error("歌单详情页面初始化失败。", ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged("IsBusy");
            OnPropertyChanged("IsNotBusy");
        }
    }
    public bool IsNotBusy => !_isBusy;


    private MyFavoriteViewModel _currentMyFavorite;
    public MyFavoriteViewModel CurrentMyFavorite
    {
        get => _currentMyFavorite;
        set
        {
            _currentMyFavorite = value;
            OnPropertyChanged();
        }
    }

    public bool IsMyFavoriteMusicsEmpty => IsBusy == false && (MyFavoriteMusics == null || MyFavoriteMusics.Count == 0);

    private ObservableCollection<MyFavoriteDetailViewModel> _myFavoriteMusics;
    public ObservableCollection<MyFavoriteDetailViewModel> MyFavoriteMusics
    {
        get => _myFavoriteMusics;
        set
        {
            _myFavoriteMusics = value;
            OnPropertyChanged();
        }
    }

    private async Task GetMyFavoriteDetail()
    {
        MyFavoriteMusics.Clear();
        var myFavoriteDetailList = await _myFavoriteService.GetMyFavoriteDetail(MyFavoriteId);
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

    private async Task LoadMyFavoriteInfo()
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

    private async void RenameMyFavorite()
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
            IsBusy = true;

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
            IsBusy = false;
        }
    }

    private async void MyFavoriteRemove()
    {
        var isOk = await Shell.Current.DisplayAlert("提示", "确定要删除该歌单吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }
        try
        {
            IsBusy = true;
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
            IsBusy = false;
        }
    }

    private async void PlayMusic(MyFavoriteDetailViewModel selected)
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
        await _playlistService.AddToPlaylist(playlist);
        await _playerService.PlayAsync(music);

        await Shell.Current.GoToAsync($"..", true);
    }

    private async void RemoveOne(MyFavoriteDetailViewModel selected)
    {
        var isOk = await Shell.Current.DisplayAlert("提示", $"确定从歌单删除吗？{Environment.NewLine}{selected.MusicName}", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            IsBusy = true;

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
            IsBusy = false;
        }
    }
}