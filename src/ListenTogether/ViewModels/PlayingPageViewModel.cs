using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class PlayingPageViewModel : ViewModelBase
{
    private readonly ILogger<PlayingPageViewModel> _logger;
    private static readonly HttpClient MyHttpClient = new HttpClient();
    //控制手动滚动歌词时，系统暂停歌词滚动
    private DateTime _lastScrollToTime = DateTime.Now;
    private readonly MusicPlayerService _playerService;
    private readonly IDispatcherTimer _timerLyricsUpdate;
    private readonly MusicNetPlatform _musicNetPlatform;
    private readonly IPlaylistService _playlistService;
    public EventHandler<LyricViewModel> ScrollToLyric { get; set; } = null!;

    /// <summary>
    /// 当前播放的歌曲
    /// </summary>
    [ObservableProperty]
    private MusicResultShowViewModel? _currentMusic;

    /// <summary>
    /// 当前播放的歌曲图片
    /// </summary>
    [ObservableProperty]
    private byte[]? _currentMusicImageByteArray;

    /// <summary>
    /// 每行的歌词
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<LyricViewModel> _lyrics = null!;

    /// <summary>
    /// 分享按钮的文本
    /// </summary>
    [ObservableProperty]
    private string _shareLabelText = Config.Desktop ? "复制歌曲链接" : "分享歌曲链接";

    public PlayingPageViewModel(MusicPlayerService playerService, MusicNetPlatform musicNetPlatform, IPlaylistService playlistService, ILogger<PlayingPageViewModel> logger)
    {
        _musicNetPlatform = musicNetPlatform;
        _playerService = playerService;
        Lyrics = new ObservableCollection<LyricViewModel>();

        _timerLyricsUpdate = App.Current.Dispatcher.CreateTimer();
        _timerLyricsUpdate.Interval = TimeSpan.FromMilliseconds(300);
        _playlistService = playlistService;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (!Config.Desktop && GlobalConfig.MyUserSetting.Play.IsPlayingPageKeepScreenOn)
            {
                DeviceDisplay.Current.KeepScreenOn = true;
            }

            _playerService.NewMusicAdded += _playerService_NewMusicAdded;
            _timerLyricsUpdate.Tick += _timerLyricsUpdate_Tick;
            _timerLyricsUpdate.Start();
            await NewMusicAddedDoAsync();
        }
        catch (Exception ex)
        {
            await ToastService.Show("正在播放的歌曲信息加载失败");
            _logger.LogError(ex, "播放页面初始化失败。");
        }
    }

    private async void _playerService_NewMusicAdded(object sender, EventArgs e)
    {
        await NewMusicAddedDoAsync();
    }
    private async Task NewMusicAddedDoAsync()
    {
        if (_playerService.Metadata == null)
        {
            return;
        }
        var playlist = await _playlistService.GetOneAsync(_playerService.Metadata.Id);
        if (playlist == null)
        {
            await ToastService.Show("歌曲信息不存在");
            return;
        }

        CurrentMusic = new MusicResultShowViewModel()
        {
            Id = playlist.Id,
            IdOnPlatform = playlist.IdOnPlatform,
            Platform = playlist.Platform,
            PlatformName = playlist.Platform.GetDescription(),
            Name = playlist.Name,
            Artist = playlist.Artist,
            Album = playlist.Album,
            ImageUrl = playlist.ImageUrl,
            ExtendDataJson = playlist.ExtendDataJson,
        };
        CurrentMusicImageByteArray = await MyHttpClient.GetByteArrayAsync(CurrentMusic.ImageUrl);
        await GetLyricDetailAsync();
    }

    /// <summary>
    /// 解析歌词
    /// </summary>
    private async Task GetLyricDetailAsync()
    {
        if (Lyrics.Count > 0)
        {
            Lyrics.Clear();
        }
        if (CurrentMusic == null)
        {
            return;
        }

        string lyric = await GetLyric();
        if (lyric.IsEmpty())
        {
            return;
        }

        string pattern = ".*";
        var lyricRowList = JiuLing.CommonLibs.Text.RegexUtils.GetAll(lyric, pattern);
        foreach (var lyricRow in lyricRowList)
        {
            if (lyricRow.IsEmpty())
            {
                continue;
            }
            pattern = @"\[(?<mm>\d*):(?<ss>\d*).(?<fff>\d*)\](?<lyric>.*)";
            var (success, result) = JiuLing.CommonLibs.Text.RegexUtils.GetMultiGroupInFirstMatch(lyricRow, pattern);
            if (success == false)
            {
                continue;
            }

            int totalMillisecond = Convert.ToInt32(result["mm"]) * 60 * 1000 + Convert.ToInt32(result["ss"]) * 1000 + Convert.ToInt32(result["fff"]);
            var info = result["lyric"];
            Lyrics.Add(new LyricViewModel() { PositionMillisecond = totalMillisecond, Info = info });
        }
    }

    private async Task<string> GetLyric()
    {
        if (CurrentMusic == null)
        {
            return default;
        }
        return await _musicNetPlatform.GetLyricAsync((NetMusicLib.Enums.PlatformEnum)CurrentMusic.Platform, CurrentMusic.IdOnPlatform, CurrentMusic.ExtendDataJson);
    }

    [RelayCommand]
    private async void ShareMusicLinkAsync()
    {
        if (CurrentMusic == null)
        {
            return;
        }

        try
        {
            string musicUrl = await _musicNetPlatform.GetPlayPageUrlAsync((NetMusicLib.Enums.PlatformEnum)CurrentMusic.Platform, CurrentMusic.IdOnPlatform);
            if (Config.Desktop)
            {
                await Clipboard.Default.SetTextAsync(musicUrl);
                await ToastService.Show($"{CurrentMusic.Name} - {CurrentMusic.Artist}{Environment.NewLine}歌曲链接已复制");
            }
            else
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Uri = musicUrl,
                    Title = $"分享歌曲链接{Environment.NewLine}{CurrentMusic.Name} - {CurrentMusic.Artist}"
                });
            }
        }
        catch (Exception ex)
        {
            await ToastService.Show("歌曲链接解析失败");
            _logger.LogError(ex, "复制歌曲链接失败。");
        }
    }

    [RelayCommand]
    private void LyricsScrolledDo()
    {
        _lastScrollToTime = DateTime.Now.AddSeconds(1);
    }

    private void _timerLyricsUpdate_Tick(object sender, EventArgs e)
    {
        if (_lastScrollToTime.Subtract(DateTime.Now).TotalMilliseconds > 0)
        {
            return;
        }

        if (CurrentMusic == null)
        {
            return;
        }
        if (_playerService.IsPlaying == false)
        {
            return;
        }
        if (Lyrics.Count == 0)
        {
            return;
        }

        try
        {
            var positionMillisecond = _playerService.CurrentPosition.position.TotalMilliseconds;
            //取大于当前进度的第一行索引，在此基础上-1则为需要高亮的行
            int highlightIndex = 0;
            foreach (var lyric in Lyrics)
            {
                lyric.IsHighlight = false;
                if (lyric.PositionMillisecond > positionMillisecond)
                {
                    break;
                }
                highlightIndex++;
            }
            if (highlightIndex > 0)
            {
                highlightIndex = highlightIndex - 1;
            }

            Lyrics[highlightIndex].IsHighlight = true;
            ScrollToLyric?.Invoke(this, Lyrics[highlightIndex]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "播放页进度更新失败。");
        }
    }

    public void OnDisappearing()
    {
        if (DeviceDisplay.Current.KeepScreenOn == true)
        {
            DeviceDisplay.Current.KeepScreenOn = false;
        }

        _playerService.NewMusicAdded -= _playerService_NewMusicAdded;
        _timerLyricsUpdate.Tick -= _timerLyricsUpdate_Tick;
        _timerLyricsUpdate.Stop();
    }
}
