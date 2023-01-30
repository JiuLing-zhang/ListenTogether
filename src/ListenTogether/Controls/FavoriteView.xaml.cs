using CommunityToolkit.Maui.Views;
using ListenTogether.Storage;

namespace ListenTogether.Controls;

public partial class FavoriteView : ContentView
{
    private bool _isLogin => UserInfoStorage.GetUsername().IsNotEmpty();
    public static readonly BindableProperty MusicIdProperty =
        BindableProperty.Create(
            nameof(MusicId),
            typeof(string),
            typeof(MusicResultView));
    public string MusicId
    {
        get { return (string)GetValue(MusicIdProperty); }
        set { SetValue(MusicIdProperty, value); }
    }

    private IMyFavoriteService? _myFavoriteService;
    private IMusicNetworkService? _musicNetworkService;
    private IPlaylistService? _playlistService;
    private IMusicService? _musicService;

    public FavoriteView()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (_myFavoriteService == null)
        {
            _myFavoriteService = this.Handler.MauiContext.Services.GetRequiredService<IMyFavoriteService>();
        }
        if (_musicNetworkService == null)
        {
            _musicNetworkService = this.Handler.MauiContext.Services.GetRequiredService<IMusicNetworkService>();
        }
        if (_playlistService == null)
        {
            _playlistService = this.Handler.MauiContext.Services.GetRequiredService<IPlaylistService>();
        }
        if (_musicService == null)
        {
            _musicService = this.Handler.MauiContext.Services.GetRequiredService<IMusicService>();
        }
        ImgFavorite.IsVisible = _isLogin;
    }
    private async void Favorite_Tapped(object sender, TappedEventArgs e)
    {
        if (!_isLogin)
        {
            await ToastService.Show("用户未登录");
            ImgFavorite.IsVisible = _isLogin;
            return;
        }

        var playlist = await _playlistService.GetOneAsync(MusicId);
        if (playlist == null)
        {
            await ToastService.Show("无法加载歌曲信息");
            return;
        }

        if (playlist.ImageUrl.IsEmpty() && playlist.Platform == Model.Enums.PlatformEnum.KuGou)
        {
            playlist.ImageUrl = await _musicNetworkService.GetImageUrlAsync(playlist.Platform, playlist.IdOnPlatform, playlist.ExtendDataJson);
        }

        var popup = new ChooseMyFavoritePage(_myFavoriteService);
        var result = await App.Current.MainPage.ShowPopupAsync(popup);
        if (result == null)
        {
            return;
        }

        if (!int.TryParse(result.ToString(), out var myFavoriteId))
        {
            await ToastService.Show("添加失败");
            return;
        }

        var localMusic = new LocalMusic()
        {
            Id = playlist.Id,
            IdOnPlatform = playlist.IdOnPlatform,
            Platform = playlist.Platform,
            Name = playlist.Name,
            Album = playlist.Album,
            Artist = playlist.Artist,
            ExtendDataJson = playlist.ExtendDataJson,
            ImageUrl = playlist.ImageUrl
        };

        var isMusicOk = await _musicService.AddOrUpdateAsync(localMusic);
        if (!isMusicOk)
        {
            await ToastService.Show("歌曲信息保存失败");
            return;
        }

        var isAddOk = await _myFavoriteService.AddMusicToMyFavoriteAsync(myFavoriteId, localMusic.Id);
        if (isAddOk == false)
        {
            await ToastService.Show("添加失败");
            return;
        }
    }
}