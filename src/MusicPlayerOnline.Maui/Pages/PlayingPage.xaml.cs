using MusicPlayerOnline.Maui.ViewModels;

namespace MusicPlayerOnline.Maui.Pages;

public partial class PlayingPage : ContentPage
{
	public PlayingPage(PlayingPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}