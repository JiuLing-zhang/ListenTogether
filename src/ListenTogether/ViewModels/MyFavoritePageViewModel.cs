using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ListenTogether.Data.Api;

namespace ListenTogether.ViewModels;
public partial class MyFavoritePageViewModel : ViewModelBase
{
    private readonly MusicResultService _musicResultService;
    private readonly IPlaylistService _playlistService;
    private readonly IMyFavoriteService _myFavoriteService;
    private readonly ILoginDataStorage _loginDataStorage;
    private readonly ILogger<MyFavoritePageViewModel> _logger;
    public string Title => "我的歌单";
    public MyFavoritePageViewModel(IPlaylistService playlistService, MusicResultService musicResultService, IMyFavoriteService myFavoriteService, ILoginDataStorage loginDataStorage, ILogger<MyFavoritePageViewModel> logger)
    {
        FavoriteList = new ObservableCollection<MyFavoriteViewModel>();

        _musicResultService = musicResultService;
        _playlistService = playlistService;
        _myFavoriteService = myFavoriteService;
        _loginDataStorage = loginDataStorage;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            IsLogin = _loginDataStorage.GetUsername().IsNotEmpty();
            if (!IsLogin)
            {
                return;
            }

            Loading("加载歌单....");

            var myFavoriteList = await _myFavoriteService.GetAllAsync();
            if (myFavoriteList == null || myFavoriteList.Count == 0)
            {
                if (FavoriteList.Count > 0)
                {
                    FavoriteList.Clear();
                }
                return;
            }

            if (myFavoriteList.Count == FavoriteList.Count)
            {
                //数据未发生变更时不更新列表
                var dbLastEditTime = myFavoriteList.OrderByDescending(x => x.EditTime).First().EditTime;
                var pageLast = FavoriteList.OrderByDescending(x => x.EditTime).FirstOrDefault();

                if (pageLast != null && pageLast.EditTime.Subtract(dbLastEditTime).TotalDays >= 0)
                {
                    return;
                }
            }

            if (FavoriteList.Count > 0)
            {
                FavoriteList.Clear();
            }
            foreach (var myFavorite in myFavoriteList)
            {
                FavoriteList.Add(new MyFavoriteViewModel()
                {
                    Id = myFavorite.Id,
                    Name = myFavorite.Name,
                    MusicCount = myFavorite.MusicCount,
                    ImageUrl = myFavorite.ImageUrl,
                    EditTime = myFavorite.EditTime
                });
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("我的歌单加载失败");
            _logger.LogError(ex, "我的歌单页面初始化失败。");
        }
        finally
        {
            LoadComplete();
        }
    }

    [ObservableProperty]
    private ObservableCollection<MyFavoriteViewModel> _favoriteList = null!;

    [RelayCommand]
    private async void AddMyFavoriteAsync()
    {
        string myFavoriteName = await App.Current.MainPage.DisplayPromptAsync("添加歌单", "请输入歌单名称：", "添加", "取消");
        if (myFavoriteName.IsEmpty())
        {
            return;
        }

        try
        {
            Loading("处理中....");
            if (await _myFavoriteService.NameExistAsync(myFavoriteName))
            {
                await ToastService.Show("歌单名称已存在");
                return;
            }

            var myFavorite = new MyFavorite()
            {
                Name = myFavoriteName,
                MusicCount = 0,
                ImageUrl = ""
            };
            var newMyFavorite = await _myFavoriteService.AddOrUpdateAsync(myFavorite);
            if (newMyFavorite == null)
            {
                await ToastService.Show("添加失败");
                return;
            }
            await InitializeAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("添加失败，网络出小差了");
            _logger.LogError(ex, "歌单添加失败。");
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    private async void EnterMyFavoriteDetailAsync(MyFavoriteViewModel selected)
    {
        await Shell.Current.GoToAsync($"{nameof(MyFavoriteDetailPage)}?{nameof(MyFavoriteDetailPageViewModel.MyFavoriteId)}={selected.Id}", true);
    }

    [RelayCommand]
    private async void PlayAllMusicsAsync(MyFavoriteViewModel selected)
    {
        if (selected.MusicCount == 0)
        {
            await ToastService.Show("当前歌单是空的哦");
            return;
        }

        var myFavoriteDetail = await _myFavoriteService.GetMyFavoriteDetailAsync(selected.Id);
        if (myFavoriteDetail == null)
        {
            await ToastService.Show("播放失败：没有查询到歌单信息~~~");
            return;
        }

        if (GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlaySongMenu)
        {
            if (!await _playlistService.RemoveAllAsync())
            {
                await ToastService.Show("播放列表清空失败");
                return;
            }
        }

        var localMusics = myFavoriteDetail.Select(x => x.Music).ToList();
        await _musicResultService.PlayAllAsync(localMusics);
    }
}