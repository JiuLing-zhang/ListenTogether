using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui.Pages;

public partial class PlaylistPage : ContentPage
{
	public PlaylistPage(PlaylistPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void Entry_Completed(object sender, EventArgs e)
    {
        //TODO 处理搜索，看看能不能通过command绑定
        //_myModel.Search();
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        //TODO 看看能不能通过command绑定
        //MenuItem menuItem = sender as MenuItem;
        //MusicDetailViewModel contextItem = menuItem.BindingContext as MusicDetailViewModel;
        //_myModel.RemovePlaylistItem(contextItem);
    }
}