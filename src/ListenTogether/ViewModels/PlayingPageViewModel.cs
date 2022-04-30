using Microsoft.Maui.Dispatching;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public class PlayingPageViewModel : ViewModelBase
{
    private IDispatcherTimer _timerLyricsUpdate;
    private readonly PlayerService _playerService;
    public EventHandler<LyricViewModel> ScrollToLyric { get; set; }
    public PlayingPageViewModel(PlayerService playerService)
    {
        _playerService = playerService;
        Lyrics = new ObservableCollection<LyricViewModel>();
        _playerService.NewMusicAdded += _playerService_NewMusicAdded;

        _timerLyricsUpdate = App.Current.Dispatcher.CreateTimer();
        _timerLyricsUpdate.Interval = TimeSpan.FromMilliseconds(300);
        _timerLyricsUpdate.Tick += _timerLyricsUpdate_Tick;
        _timerLyricsUpdate.Start();
    }

    public async Task InitializeAsync()
    {
        try
        {
            NewMusicAddedDo(_playerService.CurrentMusic);
            GetLyricDetail();
        }
        catch (Exception ex)
        {
            await ToastService.Show("正在播放的歌曲信息加载失败");
            Logger.Error("播放页面初始化失败。", ex);
        }
    }

    private Music _currentMusic;
    /// <summary>
    /// 当前播放的歌曲
    /// </summary>
    public Music CurrentMusic
    {
        get => _currentMusic;
        set
        {
            _currentMusic = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<LyricViewModel> _lyrics;
    /// <summary>
    /// 每行的歌词
    /// </summary>
    public ObservableCollection<LyricViewModel> Lyrics
    {
        get => _lyrics;
        set
        {
            _lyrics = value;
            OnPropertyChanged();
        }
    }

    private void _playerService_NewMusicAdded(object sender, Music e)
    {
        NewMusicAddedDo(e);
    }
    private void NewMusicAddedDo(Music music)
    {
        CurrentMusic = music;
    }

    /// <summary>
    /// 解析歌词
    /// </summary>
    private void GetLyricDetail()
    {
        if (Lyrics.Count > 0)
        {
            Lyrics.Clear();
        }
        if (CurrentMusic == null)
        {
            return;
        }
        if (CurrentMusic.Lyric.IsEmpty())
        {
            return;
        }

        string pattern = ".*";
        var lyricRowList = JiuLing.CommonLibs.Text.RegexUtils.GetAll(CurrentMusic.Lyric, pattern);
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

    private void _timerLyricsUpdate_Tick(object sender, EventArgs e)
    {
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
            var positionMillisecond = _playerService.PositionMillisecond;
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
}
