using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class PlaylistPageViewModel : ViewModelBase
{
    private readonly IPlaylistService _playlistService;
    private readonly MusicResultService _musicResultService;
    private readonly MusicPlayerService _musicPlayerService;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPlaylistNotEmpty))]
    private bool _isPlaylistEmpty;
    public bool IsPlaylistNotEmpty => !IsPlaylistEmpty;

    /// <summary>
    /// 播放列表
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MusicResultShowViewModel> _playlist;

    public PlaylistPageViewModel(IPlaylistService playlistService, MusicResultService musicResultService, MusicPlayerService musicPlayerService)
    {
        Playlist = new ObservableCollection<MusicResultShowViewModel>();

        _playlistService = playlistService;
        _musicResultService = musicResultService;
        _musicPlayerService = musicPlayerService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            Loading("加载中....");
            await GetPlaylistAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("播放列表加载失败");
            Logger.Error("播放列表页面初始化失败。", ex);
        }
        finally
        {
            LoadComplete();
        }
    }
    private async Task GetPlaylistAsync()
    {
        var playlist = await _playlistService.GetAllAsync();
        if (playlist.Count == 0)
        {
            if (Playlist.Count > 0)
            {
                Playlist.Clear();
            }
            IsPlaylistEmpty = !Playlist.Any();

            return;
        }

        if (Playlist.Count > 0)
        {
            Playlist.Clear();
        }
        foreach (var item in playlist)
        {
            Playlist.Add(new MusicResultShowViewModel()
            {
                Platform = item.Platform,
                PlatformName = item.Platform.GetDescription(),
                Id = item.Id,
                Name = item.Name,
                Artist = item.Artist,
                Album = item.Album,
            });
        }

        IsPlaylistEmpty = !Playlist.Any();
    }

    [RelayCommand]
    private async void PlayMusicAsync(MusicResultShowViewModel selected)
    {
        await _musicResultService.PlayAsync(selected.ToLocalMusic());
    }

    [RelayCommand]
    private async void RemoveOneAsync(MusicResultShowViewModel selected)
    {
        try
        {
            Loading("正在删除....");

            if (_musicPlayerService.IsPlaying)
            {
                if (Playlist.Count == 1)
                {
                    //播放列表仅剩当前歌曲时，直接暂停播放
                    await _musicPlayerService.PlayAsync(_musicPlayerService.Metadata.Id);
                }
                else if (_musicPlayerService.Metadata.Id == selected.Id)
                {
                    await _musicPlayerService.Next();
                }
            }

            if (!await _playlistService.RemoveAsync(selected.Id))
            {
                await ToastService.Show("删除失败");
                return;
            }

            await GetPlaylistAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("删除失败，网络出小差了");
            Logger.Error("播放列表删除歌曲失败。", ex);
        }
        finally
        {
            LoadComplete();
        }
    }

    [RelayCommand]
    private async void ClearPlaylistAsync()
    {
        var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除播放列表吗？", "确定", "取消");
        if (isOk == false)
        {
            return;
        }

        if (_musicPlayerService.IsPlaying)
        {
            await _musicPlayerService.PlayAsync(_musicPlayerService.Metadata.Id);
        }

        if (!await _playlistService.RemoveAllAsync())
        {
            await ToastService.Show("删除失败");
            return;
        }
        await GetPlaylistAsync();
    }
}
