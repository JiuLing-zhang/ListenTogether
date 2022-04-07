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
    public ICommand SelectedChangedCommand => new Command<MusicViewModel>(SelectedChangedDo);
    public ICommand MyFavoriteEditCommand => new Command(EditMyFavorite);
    public ICommand MyFavoriteRemoveCommand => new Command(MyFavoriteRemove);

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

    private async void EditMyFavorite()
    {
        await Shell.Current.GoToAsync($"{nameof(MyFavoriteEditPage)}?{nameof(MyFavoriteEditPageViewModel.MyFavoriteId)}={MyFavoriteId}", true);
    }

    private async void MyFavoriteRemove()
    {
        var isOk = await Shell.Current.DisplayAlert("提示", "确定要删除该歌单吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        var result = await _myFavoriteService.RemoveAsync(MyFavoriteId);
        if (result == false)
        {
            ToastService.Show("删除成功");
            return;
        }
        ToastService.Show("删除成功");
        await Shell.Current.GoToAsync($"..", true);
    }

    private async void SelectedChangedDo(MusicViewModel selected)
    {
        var music = await _musicService.GetOneAsync(selected.Id);
        if (music == null)
        {
            ToastService.Show("获取歌曲信息失败");
            return;
        }

        var playlist = new Playlist()
        {
            MusicId = music.Id,
            MusicName = music.Name,
            MusicArtist = music.Artist
        };
        await _playlistService.AddToPlaylist(playlist);

        //TODO 播放
        //if (await GlobalMethods.PlayMusic(music) == false)
        //{
        //    return;
        //}
        await Shell.Current.GoToAsync($"{nameof(PlayingPage)}", true);

        //TODO 更新播放列表
        //MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
    }


}