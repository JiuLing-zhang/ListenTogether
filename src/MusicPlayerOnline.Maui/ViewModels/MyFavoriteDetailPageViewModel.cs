using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels;

[QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
public class MyFavoriteDetailPageViewModel : ViewModelBase
{
    public int MyFavoriteId { get; set; }

    private IServiceProvider _services;
    private IMyFavoriteService _myFavoriteService;
    private IPlaylistService _playlistService;
    private IMusicService _musicService;
    private PlayerService _playerService;
    public ICommand PlayMusicCommand => new Command<MusicViewModel>(PlayMusic);
    public ICommand MyFavoriteRenameCommand => new Command(RenameMyFavorite);
    public ICommand MyFavoriteRemoveCommand => new Command(MyFavoriteRemove);
    public ICommand RemoveOneCommand => new Command<MusicViewModel>(RemoveOne);

    public MyFavoriteDetailPageViewModel(IServiceProvider services, PlayerService playerService)
    {
        MyFavoriteMusics = new ObservableCollection<MusicViewModel>();

        _services = services;
        _playerService = playerService;
    }

    public async Task InitializeAsync()
    {
        IsBusy = true;
        _myFavoriteService = _services.GetService<IMyFavoriteServiceFactory>().Create();
        _playlistService = _services.GetService<IPlaylistServiceFactory>().Create();
        _musicService = _services.GetService<IMusicServiceFactory>().Create();

        await LoadMyFavoriteInfo();
        await GetMyFavoriteDetail();

        OnPropertyChanged("IsMyFavoriteMusicsEmpty");
        IsBusy = false;
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

    private ObservableCollection<MusicViewModel> _myFavoriteMusics;
    public ObservableCollection<MusicViewModel> MyFavoriteMusics
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
            MyFavoriteMusics.Add(new MusicViewModel()
            {
                Seq = ++seq,
                Id = myFavoriteDetail.MusicId,
                Platform = myFavoriteDetail.Platform.GetDescription(),
                Artist = myFavoriteDetail.MusicArtist,
                Album = myFavoriteDetail.MusicAlbum,
                Name = myFavoriteDetail.MusicName
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
            await ToastService.Show("修改成功");
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
            await ToastService.Show("修改成功");
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
            await ToastService.Show("删除成功");
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

    private async void PlayMusic(MusicViewModel selected)
    {
        var music = await _musicService.GetOneAsync(selected.Id);
        if (music == null)
        {
            await ToastService.Show("获取歌曲信息失败");
            return;
        }

        var playlist = new Playlist()
        {
            MusicId = music.Id,
            MusicName = music.Name,
            MusicArtist = music.Artist
        };
        await _playlistService.AddToPlaylist(playlist);
        await _playerService.PlayAsync(music);

        await Shell.Current.GoToAsync($"..", true);
    }

    private async void RemoveOne(MusicViewModel selected)
    {
        try
        {
            IsBusy = true;

            //TODO 实现删除功能
            await ToastService.Show("删除成功");
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