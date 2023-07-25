using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

[QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
public partial class MyFavoriteDetailPageViewModel : ViewModelBase
{
    private readonly ILogger<MyFavoriteDetailPageViewModel> _logger;
    public int MyFavoriteId { get; set; }

    private readonly MusicResultService _musicResultService;
    private readonly IMyFavoriteService _myFavoriteService;

    [ObservableProperty]
    private MyFavoriteViewModel _currentMyFavorite = null!;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMyFavoriteMusicsEmpty))]
    private ObservableCollection<MyFavoriteDetailViewModel> _myFavoriteMusics = null!;
    public bool IsMyFavoriteMusicsEmpty => MyFavoriteMusics == null || MyFavoriteMusics.Count == 0;

    public MyFavoriteDetailPageViewModel(MusicResultService musicResultService, IMyFavoriteService myFavoriteService, ILogger<MyFavoriteDetailPageViewModel> logger)
    {
        MyFavoriteMusics = new ObservableCollection<MyFavoriteDetailViewModel>();

        _musicResultService = musicResultService;
        _myFavoriteService = myFavoriteService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            Loading("加载中....");
            await LoadMyFavoriteInfoAsync();
            await GetMyFavoriteDetailAsync();

        }
        catch (Exception ex)
        {
            await ToastService.Show("歌单详情加载失败");
            _logger.LogError(ex, "歌单详情页面初始化失败。");
        }
        finally
        {
            LoadComplete();
        }
    }
    private async Task GetMyFavoriteDetailAsync()
    {
        MyFavoriteMusics.Clear();
        var myFavoriteDetailList = await _myFavoriteService.GetMyFavoriteDetailAsync(MyFavoriteId);
        if (myFavoriteDetailList == null)
        {
            return;
        }
        int seq = 0;
        foreach (var myFavoriteDetail in myFavoriteDetailList)
        {
            MyFavoriteMusics.Add(new MyFavoriteDetailViewModel()
            {
                Seq = ++seq,
                Id = myFavoriteDetail.Id,
                Music = new MusicResultShowViewModel()
                {
                    Id = myFavoriteDetail.Music.Id,
                    PlatformName = myFavoriteDetail.Music.Platform.GetDescription(),
                    Artist = myFavoriteDetail.Music.Artist,
                    Album = myFavoriteDetail.Music.Album,
                    Name = myFavoriteDetail.Music.Name,
                    IdOnPlatform = myFavoriteDetail.Music.Id,
                    ImageUrl = myFavoriteDetail.Music.ImageUrl,
                    Platform = myFavoriteDetail.Music.Platform,
                    ExtendDataJson = myFavoriteDetail.Music.ExtendDataJson
                }
            });
        }
    }

    private async Task LoadMyFavoriteInfoAsync()
    {
        var myFavorite = await _myFavoriteService.GetOneAsync(MyFavoriteId);
        if (myFavorite == null)
        {
            return;
        }
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
            Loading("处理中....");

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
            _logger.LogError(ex, "歌单重命名失败。");
        }
        finally
        {
            LoadComplete();
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
            Loading("处理中....");
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
            _logger.LogError(ex, "歌单删除失败。");
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    private async void PlayMusicAsync(MyFavoriteDetailViewModel selected)
    {
        await _musicResultService.PlayAsync(selected.Music.ToLocalMusic());
        await Shell.Current.GoToAsync($"..", true);
    }

    [RelayCommand]
    private async void RemoveOneAsync(MyFavoriteDetailViewModel selected)
    {
        var isOk = await Shell.Current.DisplayAlert("提示", $"确定从歌单删除吗？{Environment.NewLine}{selected.Music.Name}", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        try
        {
            Loading("处理中....");

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
            _logger.LogError(ex, "我的歌单删除歌曲失败。");
        }
        finally
        {
            LoadComplete();
        }
    }
}