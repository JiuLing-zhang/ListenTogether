using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;

namespace ListenTogether.Pages;

public partial class ChooseMyFavoritePage : Popup
{
    private readonly IMyFavoriteService _myFavoriteService;
    public ObservableCollection<MyFavoriteViewModel> MyFavoriteCollection { get; set; }
    public ChooseMyFavoritePage(IMyFavoriteService myFavoriteService)
    {
        _myFavoriteService = myFavoriteService;

        MyFavoriteCollection = new ObservableCollection<MyFavoriteViewModel>();
        var task = Task.Run(_myFavoriteService.GetAllAsync);
        var myFavorites = task.Result;
        foreach (var myFavorite in myFavorites)
        {
            MyFavoriteCollection.Add(new MyFavoriteViewModel()
            {
                Id = myFavorite.Id,
                Name = myFavorite.Name
            });
        }

        InitializeComponent();
    }

    private void BtnClose_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private void MyFavorite_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e == null || e.CurrentSelection == null || e.CurrentSelection.Count != 1)
        {
            return;
        }
        var obj = e.CurrentSelection[0] as MyFavoriteViewModel;
        if (obj == null)
        {
            return;
        }
        Close(obj.Id);
    }

    private async void AddMyFavorite_Tapped(object sender, TappedEventArgs e)
    {
        string myFavoriteName = await App.Current.MainPage.DisplayPromptAsync("添加歌单", "请输入歌单名称：", "添加", "取消");
        if (myFavoriteName.IsEmpty())
        {
            return;
        }

        if (await _myFavoriteService.NameExistAsync(myFavoriteName))
        {
            await ToastService.Show("歌单名称已存在");
            return;
        }

        var myFavorite = new MyFavorite()
        {
            Name = myFavoriteName,
            MusicCount = 0
        };
        var newMyFavorite = await _myFavoriteService.AddOrUpdateAsync(myFavorite);
        if (newMyFavorite == null)
        {
            await ToastService.Show("添加失败");
            return;
        }

        Close(newMyFavorite.Id);
    }
}