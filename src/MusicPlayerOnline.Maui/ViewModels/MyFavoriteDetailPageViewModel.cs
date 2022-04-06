using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Maui.Pages;
using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels;

[QueryProperty(nameof(MyFavoriteId), nameof(MyFavoriteId))]
public class MyFavoriteDetailPageViewModel : ViewModelBase
{
    public int MyFavoriteId { get; set; }

    private readonly IMyFavoriteService _myFavoriteService;
    private readonly IPlaylistService _playlistService;
    private readonly IMusicService _musicService;
    public Command SelectedChangedCommand => new Command(SelectedChangedDo);
    public Command PlayAllMusicsCommand => new Command(PlayAllMusics);

    public MyFavoriteDetailPageViewModel(IMyFavoriteServiceFactory myFavoriteServiceFactory, IPlaylistServiceFactory playlistServiceFactory, IMusicServiceFactory musicServiceFactory)
    {
        MyFavoriteMusics = new ObservableCollection<MusicViewModel>();

        _myFavoriteService = myFavoriteServiceFactory.Create();
        _playlistService = playlistServiceFactory.Create();
        _musicService = musicServiceFactory.Create();
    }

    public async Task InitializeAsync()
    {
        await LoadPageTitle();
        await GetMyFavoriteDetail();
    }

    private string _title;
    /// <summary>
    /// 页面标题
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    private MusicViewModel _selectedItem;
    public MusicViewModel SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<MusicViewModel> _myFavoriteMusics;
    public ObservableCollection<MusicViewModel> MyFavoriteMusics
    {
        get => _myFavoriteMusics;
        set
        {
            _myFavoriteMusics = value;
            OnPropertyChanged();
        }
    }

    private async Task GetMyFavoriteDetail()
    {
        MyFavoriteMusics.Clear();
        var myFavoriteDetailList = await _myFavoriteService.GetMyFavoriteDetail(MyFavoriteId);
        int seq = 0;
        foreach (var myFavoriteDetail in myFavoriteDetailList)
        {
            MyFavoriteMusics.Add(new MusicViewModel()
            {
                Seq = ++seq,
                Id = myFavoriteDetail.MusicId,
                Platform = myFavoriteDetail.Platform.GetDescription(),
                Artist = myFavoriteDetail.MusicArtist,
                Album = myFavoriteDetail.MusicAlbum,
                Name = myFavoriteDetail.MusicName
            });
        }
    }

    private async Task LoadPageTitle()
    {
        var myFavorite = await _myFavoriteService.GetOneAsync(MyFavoriteId);
        Title = myFavorite.Name;
    }

    private async void SelectedChangedDo()
    {
        var music = await _musicService.GetOneAsync(SelectedItem.Id);
        if (music == null)
        {
            ToastService.Show("获取歌曲信息失败");
            return;
        }

        await _playlistService.AddToPlaylist(music);

        //TODO 播放
        //if (await GlobalMethods.PlayMusic(music) == false)
        //{
        //    return;
        //}
        await Shell.Current.GoToAsync($"//{nameof(PlayingPage)}", true);

        //TODO 更新播放列表
        //MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
    }

    private async void PlayAllMusics()
    {
        if (GlobalConfig.MyUserSetting.Play.IsCleanPlaylistWhenPlayMyFavorite)
        {
            await _playlistService.RemoveAllAsync();
        }

        int index = 0;
        foreach (var myFavoriteMusic in MyFavoriteMusics)
        {
            var music = await _musicService.GetOneAsync(myFavoriteMusic.Id);
            if (music == null)
            {
                ToastService.Show("获取歌曲信息失败");
                return;
            }
            await _playlistService.AddToPlaylist(music);
            if (index == 0)
            {
                //TODO 播放
                //if (await GlobalMethods.PlayMusic(music) == false)
                //{
                //    return;
                //}
            }
            index++;
        }

        await Shell.Current.GoToAsync($"//{nameof(PlayingPage)}", true);
        //TODO 更新播放列表
        //MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
    }
}