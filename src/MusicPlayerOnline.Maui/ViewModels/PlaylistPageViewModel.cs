using JiuLing.CommonLibs.ExtensionMethods;
using System.Collections.ObjectModel;

namespace MusicPlayerOnline.Maui.ViewModels;

public class PlaylistPageViewModel : ViewModelBase
{
    private IServiceProvider _services;
    private IMusicService _musicService;
    private IPlaylistService _playlistService;
    private readonly PlayerService _playerService;
    public ICommand AddToMyFavoriteCommand => new Command<MusicViewModel>(AddToMyFavorite);
    public ICommand PlayMusicCommand => new Command<MusicViewModel>(PlayMusic);
    public ICommand ClearPlaylistCommand => new Command(ClearPlaylist);
    public PlaylistPageViewModel(IServiceProvider services, PlayerService playerService)
    {
        CreateLocalNewPlaylist();

        _services = services;
        _playerService = playerService;
    }

    private void CreateLocalNewPlaylist()
    {
        Playlist = new ObservableCollection<MusicViewModel>();
    }

    public async Task InitializeAsync()
    {
        _playlistService = _services.GetService<IPlaylistServiceFactory>().Create();
        _musicService = _services.GetService<IMusicServiceFactory>().Create();
        await GetPlaylist();
    }
    private async Task GetPlaylist()
    {
        if (Playlist.Count > 0)
        {
            Playlist.Clear();
        }
        var playlist = await _playlistService.GetAllAsync();
        foreach (var item in playlist)
        {
            Playlist.Add(new MusicViewModel()
            {
                Id = item.MusicId,
                Name = item.MusicName,
                Artist = item.MusicArtist
            });
        }

    }

    /// <summary>
    /// 页面标题
    /// </summary>
    public string Title => "播放列表";

    private string _searchKeyword;
    /// <summary>
    /// 搜索关键字
    /// </summary>
    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            _searchKeyword = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<MusicViewModel> _playlist;
    /// <summary>
    /// 搜索到的结果列表
    /// </summary>
    public ObservableCollection<MusicViewModel> Playlist
    {
        get => _playlist;
        set
        {
            _playlist = value;
            OnPropertyChanged();
        }
    }

    private async void PlayMusic(MusicViewModel selectedMusic)
    {
        var music = await _musicService.GetOneAsync(selectedMusic.Id);
        if (music == null)
        {
            await ToastService.Show("获取歌曲信息失败");
            return;
        }

        await _playerService.PlayAsync(music);
    }

    private async void AddToMyFavorite(MusicViewModel music)
    {
        if (music == null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedMusicId)}={music.Id}", true);
    }

    public async void RemovePlaylistItem(MusicViewModel music)
    {
        if (music == null)
        {
            return;
        }

        //TODO 删除一条
        //await _playlistService.RemoveAsync(music.Id);
        await GetPlaylist();
    }

    private async void ClearPlaylist()
    {
        if (Playlist.Count == 0)
        {
            ToastService.Show("别删除了，播放列表是空哒");
            return;
        }

        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除播放列表吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }
        if (!await _playlistService.RemoveAllAsync())
        {
            ToastService.Show("删除失败");
            return;
        }
        ToastService.Show("播放列表已删除");
        CreateLocalNewPlaylist();
    }
}
