using JiuLing.CommonLibs.ExtensionMethods;

namespace MusicPlayerOnline.Maui.ViewModels;


[QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
public class MyFavoriteEditPageViewModel : ViewModelBase
{
    public int MyFavoriteId { get; set; }

    private IServiceProvider _services;

    private IMyFavoriteService _myFavoriteService;

    public ICommand RenameCommand => new Command(Rename);

    public MyFavoriteEditPageViewModel(IServiceProvider services)
    {
        _services = services;
    }

    public async Task InitializeAsync()
    {
        _myFavoriteService = _services.GetService<IMyFavoriteServiceFactory>().Create();
        await GetMyFavoriteDetail();
    }

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _newName;
    public string NewName
    {
        get => _newName;
        set
        {
            _newName = value;
            OnPropertyChanged();
        }
    }

    private string _imageUrl;
    public string ImageUrl
    {
        get => _imageUrl;
        set
        {
            _imageUrl = value;
            OnPropertyChanged();
        }
    }

    private async Task GetMyFavoriteDetail()
    {
        var myFavorite = await _myFavoriteService.GetOneAsync(MyFavoriteId);
        Name = myFavorite.Name;
        ImageUrl = myFavorite.ImageUrl.IsEmpty() ? "icon" : myFavorite.ImageUrl;
    }
    private async void Rename()
    {
        if (NewName.IsEmpty())
        {
            ToastService.Show("输入歌单名称");
            return;
        }

        var myFavorite = await _myFavoriteService.GetOneAsync(MyFavoriteId);
        if (myFavorite.Name == NewName)
        {
            await Shell.Current.GoToAsync($"..", true);
            ToastService.Show("修改成功");
            return;
        }

        myFavorite.Name = NewName;
        await _myFavoriteService.AddOrUpdateAsync(myFavorite);

        ToastService.Show("修改成功");
        await Shell.Current.GoToAsync($"..", true);
    }
}