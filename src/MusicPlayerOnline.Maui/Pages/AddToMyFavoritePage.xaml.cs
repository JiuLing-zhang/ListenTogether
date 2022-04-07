namespace MusicPlayerOnline.Maui;

public partial class AddToMyFavoritePage : ContentPage
{
	AddToMyFavoritePageViewModel vm => BindingContext as AddToMyFavoritePageViewModel;
	public AddToMyFavoritePage(AddToMyFavoritePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await vm.InitializeAsync();
	}
}