using CommunityToolkit.Maui.Views;

namespace ListenTogether.Pages;

public partial class ChooseMyFavoritePage : Popup
{
    private List<MyFavorite> _myFavorites = new List<MyFavorite>();
    public List<string> MyFavoriteName { get; set; }
    private readonly IMyFavoriteService _myFavoriteService;
    public ChooseMyFavoritePage(IMyFavoriteService myFavoriteService)
    {
        InitializeComponent();
        _myFavoriteService = myFavoriteService;
    }

    public async Task BuildFavoriteListAsync()
    {
        _myFavorites = await _myFavoriteService.GetAllAsync();
        MyFavoriteName = _myFavorites.Select(x => x.Name).ToList();
    }

    private void BtnClose_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}