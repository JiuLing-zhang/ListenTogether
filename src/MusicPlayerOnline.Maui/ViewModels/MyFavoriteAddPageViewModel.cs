using JiuLing.CommonLibs.ExtensionMethods;

namespace MusicPlayerOnline.Maui.ViewModels;

[QueryProperty(nameof(AddedMusicId), nameof(AddedMusicId))]
public class MyFavoriteAddPageViewModel : ViewModelBase
{
    /// <summary>
    /// 要添加的歌曲ID
    /// </summary>
    public string AddedMusicId { get; set; }

    private readonly IMyFavoriteService _myFavoriteService;
    private readonly IMusicService _musicService;

    public ICommand SaveMyFavoriteCommand => new Command(SaveMyFavorite);

    public MyFavoriteAddPageViewModel(IMusicServiceFactory musicServiceFactory, IMyFavoriteServiceFactory myFavoriteServiceFactory)
    {
        _myFavoriteService = myFavoriteServiceFactory.Create();
        _musicService = musicServiceFactory.Create();
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

    private async void SaveMyFavorite()
    {
        if (Name.IsEmpty())
        {
            ToastService.Show("输入歌单名称");
            return;
        }

        if (await _myFavoriteService.NameExist(Name))
        {
            ToastService.Show("歌单名称已存在");
            return;
        }

        var myFavorite = new MyFavorite()
        {
            Name = Name,
            MusicCount = 0
        };
        var newMyFavorite = await _myFavoriteService.AddOrUpdateAsync(myFavorite);
        if (newMyFavorite == null)
        {
            ToastService.Show("添加失败");
            return;
        }

        if (AddedMusicId.IsEmpty())
        {
            //不需要添加歌曲到歌单时，直接退出
            await Shell.Current.GoToAsync($"..", true);
            return;
        }

        //添加完歌单后需要将歌曲添加到歌单
        var music = await _musicService.GetOneAsync(AddedMusicId);

        var result = await _myFavoriteService.AddMusicToMyFavorite(newMyFavorite.Id, music);
        if (result == false)
        {
            ToastService.Show("添加失败");
        }
        await Shell.Current.GoToAsync("..", false);
        await Shell.Current.GoToAsync("..", true);
    }
}