namespace MusicPlayerOnline.Maui.Pages;

public partial class PlaylistPage : ContentPage
{
    PlaylistPageViewModel vm => BindingContext as PlaylistPageViewModel;
    public PlaylistPage(PlaylistPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
        await vm.InitializeAsync();
    }
    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        //TODO 看看能不能通过command绑定
        //MenuItem menuItem = sender as MenuItem;
        //MusicDetailViewModel contextItem = menuItem.BindingContext as MusicDetailViewModel;
        //_myModel.RemovePlaylistItem(contextItem);
    }
}