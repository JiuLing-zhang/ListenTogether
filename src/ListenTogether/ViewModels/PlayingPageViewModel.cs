using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListenTogether.Model;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public partial class PlayingPageViewModel : ViewModelBase
{
    //控制手动滚动歌词时，系统暂停歌词滚动
    private DateTime _lastScrollToTime = DateTime.Now;

    private readonly PlayerService _playerService = null!;
    private readonly IDispatcherTimer _timerLyricsUpdate = null!;
    private readonly IMusicNetworkService _musicNetworkService = null!;
    public EventHandler<LyricViewModel> ScrollToLyric { get; set; } = null!;

    public PlayingPageViewModel(PlayerService playerService, IMusicNetworkService musicNetworkService)
    {
        _musicNetworkService = musicNetworkService;
        _playerService = playerService;
        Lyrics = new ObservableCollection<LyricViewModel>();

        _timerLyricsUpdate = App.Current.Dispatcher.CreateTimer();
        _timerLyricsUpdate.Interval = TimeSpan.FromMilliseconds(300);
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
            NewMusicAddedDo();
        }
        catch (Exception ex)
        {
            await ToastService.Show("正在播放的歌曲信息加载失败");
            Logger.Error("播放页面初始化失败。", ex);
        }
    }

    /// <summary>
    /// 当前播放的歌曲
    /// </summary>
    [ObservableProperty]
    private Music _currentMusic = null!;

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

    private void _playerService_NewMusicAdded(object sender, EventArgs e)
    {
        NewMusicAddedDo();
    }
    private void NewMusicAddedDo()
    {
        CurrentMusic = _playerService.CurrentMusic;
        GetLyricDetail();
    }

    /// <summary>
    /// 解析歌词
    /// </summary>
    private async void GetLyricDetail()
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
        return await _musicNetworkService.GetLyricAsync(CurrentMusic);
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
            string musicUrl = await _musicNetworkService.GetPlayPageUrlAsync(CurrentMusic);
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
            Logger.Error("复制歌曲链接失败。", ex);
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
            Logger.Error("播放页进度更新失败。", ex);
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
