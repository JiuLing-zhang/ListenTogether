using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;
public partial class MyFavoritePageViewModel : ViewModelBase
{
    private readonly PlayerService _playerService;
    private readonly IServiceProvider _services;
    private readonly IPlaylistService _playlistService;
    private IMyFavoriteService _myFavoriteService;
    private IMusicService _musicService;

    public string Title => "我的歌单";
    public MyFavoritePageViewModel(IServiceProvider services, IPlaylistService playlistService, PlayerService playerService)
    {
        FavoriteList = new ObservableCollection<MyFavoriteViewModel>();
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
            Logger.Error("我的歌单页面初始化失败。", ex);
        }
        finally
        {
            StopLoading();
        }
    }

    [ObservableProperty]
    private ObservableCollection<MyFavoriteViewModel> _favoriteList;

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
            StartLoading("处理中....");
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
            Logger.Error("歌单添加失败。", ex);
        }
        finally
        {
            StopLoading();
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

        var myFavoriteMusics = await _myFavoriteService.GetMyFavoriteDetailAsync(selected.Id);
        if (myFavoriteMusics == null)
        {
            await ToastService.Show("播放失败：没有查询到歌单信息~~~");
            return;
        }

        if (GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite)
        {
            await _playlistService.RemoveAllAsync();
        }

        foreach (var myFavoriteMusic in myFavoriteMusics)
        {
            var playlist = new Playlist()
            {
                PlatformName = myFavoriteMusic.PlatformName,
                MusicId = myFavoriteMusic.MusicId,
                MusicName = myFavoriteMusic.MusicName,
                MusicArtist = myFavoriteMusic.MusicArtist,
                MusicAlbum = myFavoriteMusic.MusicAlbum
            };

            await _playlistService.AddToPlaylistAsync(playlist);
        }

        if (myFavoriteMusics.Count > 0)
        {
            var music = await _musicService.GetOneAsync(myFavoriteMusics[0].MusicId);
            await _playerService.PlayAsync(music);
        }
    }
}