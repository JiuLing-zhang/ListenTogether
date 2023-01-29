using CommunityToolkit.Maui.Views;
using ListenTogether.Storage;

namespace ListenTogether.Controls;

public partial class FavoriteView : ContentView
{
    public bool IsLogin => UserInfoStorage.GetUsername().IsNotEmpty();

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

    private UserFavoriteService? _userFavoriteService;
    private IMyFavoriteService? _myFavoriteService;
    public FavoriteView()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (!IsLogin)
        {
            return;
        }
        if (_myFavoriteService == null)
        {
            _myFavoriteService = this.Handler.MauiContext.Services.GetRequiredService<IMyFavoriteService>();
        }
        if (_userFavoriteService == null)
        {
            _userFavoriteService = this.Handler.MauiContext.Services.GetRequiredService<UserFavoriteService>();
        }
        SetFavoriteImage();
    }

    private void SetFavoriteImage()
    {
        var isExist = _userFavoriteService.GetMusicsId().Contains(MusicId);
        if (isExist)
        {
            ImgFavorite.Source = "favorite.png";
        }
        else
        {
            if (Config.IsDarkTheme)
            {
                ImgFavorite.Source = "un_favorite_dark.png";
            }
            else
            {
                ImgFavorite.Source = "un_favorite.png";
            }
        }
    }

    private async void Favorite_Tapped(object sender, TappedEventArgs e)
    {
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
        //TODO
    }
}