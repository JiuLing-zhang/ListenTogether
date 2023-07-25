using CommunityToolkit.Maui.Views;
using ListenTogether.Data.Api;

namespace ListenTogether.Controls;

public partial class FavoriteView : ContentView
{
    public static readonly BindableProperty MusicProperty =
        BindableProperty.Create(
            nameof(Music),
            typeof(MusicResultShowViewModel),
            typeof(MusicResultView));
    public MusicResultShowViewModel Music
    {
        get { return (MusicResultShowViewModel)GetValue(MusicProperty); }
        set { SetValue(MusicProperty, value); }
    }

    private IMyFavoriteService? _myFavoriteService;
    private MusicNetPlatform? _musicNetworkService;
    private IPlaylistService? _playlistService;
    private IMusicService? _musicService;
    private ILoginDataStorage? _loginDataStorage;
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
            _musicNetworkService = this.Handler.MauiContext.Services.GetRequiredService<MusicNetPlatform>();
        }
        if (_playlistService == null)
        {
            _playlistService = this.Handler.MauiContext.Services.GetRequiredService<IPlaylistService>();
        }
        if (_musicService == null)
        {
            _musicService = this.Handler.MauiContext.Services.GetRequiredService<IMusicService>();
        }
        if (_loginDataStorage == null)
        {
            _loginDataStorage = this.Handler.MauiContext.Services.GetRequiredService<ILoginDataStorage>();
        }
        ImgFavorite.IsVisible = IsLogin();
    }
    private bool IsLogin()
    {
        if (_loginDataStorage == null)
        {
            return false;
        }
        return _loginDataStorage.GetUsername().IsNotEmpty();
    }
    private async void Favorite_Tapped(object sender, TappedEventArgs e)
    {
        if (!IsLogin())
        {
            await ToastService.Show("用户未登录");
            ImgFavorite.IsVisible = IsLogin();
            return;
        }

        var localMusic = new LocalMusic()
        {
            Id = Music.Id,
            IdOnPlatform = Music.IdOnPlatform,
            Platform = Music.Platform,
            Name = Music.Name,
            Album = Music.Album,
            Artist = Music.Artist,
            ExtendDataJson = Music.ExtendDataJson,
            ImageUrl = Music.ImageUrl
        };

        if (localMusic.ImageUrl.IsEmpty() && localMusic.Platform == Model.Enums.PlatformEnum.KuGou)
        {
            localMusic.ImageUrl = await _musicNetworkService.GetImageUrlAsync((NetMusicLib.Enums.PlatformEnum)localMusic.Platform, localMusic.IdOnPlatform, localMusic.ExtendDataJson);
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