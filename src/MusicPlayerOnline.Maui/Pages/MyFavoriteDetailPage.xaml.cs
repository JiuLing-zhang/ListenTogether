namespace MusicPlayerOnline.Maui;

public partial class MyFavoriteDetailPage : ContentPage
{
	MyFavoriteDetailPageViewModel vm => BindingContext as MyFavoriteDetailPageViewModel;
	public MyFavoriteDetailPage(MyFavoriteDetailPageViewModel vm)
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