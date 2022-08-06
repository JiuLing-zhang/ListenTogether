using ListenTogether.Network.Models.KuGou;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Collections.ObjectModel;

namespace ListenTogether.ViewModels;

public class PlayingPageViewModel : ViewModelBase
{
    //控制手动滚动歌词时，系统暂停歌词滚动
    private DateTime _lastScrollToTime = DateTime.Now;
    private IDispatcherTimer _timerLyricsUpdate;
    private readonly PlayerService _playerService;
    public EventHandler<LyricViewModel> ScrollToLyric { get; set; }

    public ICommand CopyMusicLinkCommand => new Command(CopyMusicLink);
    public ICommand ShareMusicLinkCommand => new Command(ShareMusicLink);
    public ICommand LyricsScrolledCommand => new Command(LyricsScrolledDo);
    public PlayingPageViewModel(PlayerService playerService)
    {
        _playerService = playerService;
        Lyrics = new ObservableCollection<LyricViewModel>();

        _timerLyricsUpdate = App.Current.Dispatcher.CreateTimer();
        _timerLyricsUpdate.Interval = TimeSpan.FromMilliseconds(300);
    }

    public async Task InitializeAsync()
    {
        try
        {
            _playerService.NewMusicAdded += _playerService_NewMusicAdded;
            _timerLyricsUpdate.Tick += _timerLyricsUpdate_Tick;
            _timerLyricsUpdate.Start();
            NewMusicAddedDo(_playerService.CurrentMusic);
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

    private void _playerService_NewMusicAdded(object sender, EventArgs e)
    {
        NewMusicAddedDo(_playerService.CurrentMusic);
    }
    private void NewMusicAddedDo(Music music)
    {
        CurrentMusic = music;
        GetLyricDetail();
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

    private async void CopyMusicLink()
    {
        try
        {
            string musicUrl = GetCurrentMusicLink();
            await Clipboard.Default.SetTextAsync(musicUrl);
            await ToastService.Show($"歌曲链接已复制");
        }
        catch (Exception ex)
        {
            await ToastService.Show("歌曲链接解析失败");
            Logger.Error("复制歌曲链接失败。", ex);
        }
    }

    private async void ShareMusicLink()
    {
        try
        {
            string uri = GetCurrentMusicLink();
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = "Share Web Link"
            });
        }
        catch (Exception ex)
        {
            await ToastService.Show("歌曲链接解析失败");
            Logger.Error("分享歌曲链接失败。", ex);
        }
    }

    private string GetCurrentMusicLink()
    {
        if (CurrentMusic == null)
        {
            return default;
        }

        var music = CurrentMusic;
        string musicUrl;
        switch (music.Platform)
        {
            case Model.Enums.PlatformEnum.NetEase:
                musicUrl = GetNetEaseMusicUrl(music.PlatformInnerId);
                break;
            case Model.Enums.PlatformEnum.KuGou:
                musicUrl = GetKuGouMusicUrl(music.PlatformInnerId, music.ExtendData);
                break;
            case Model.Enums.PlatformEnum.MiGu:
                musicUrl = GetMiGuMusicUrl(music.PlatformInnerId);
                break;
            case Model.Enums.PlatformEnum.KuWo:
                musicUrl = GetKuWoMusicUrl(music.PlatformInnerId);
                break;
            default:
                return default;
        }
        return musicUrl;
    }

    private string GetNetEaseMusicUrl(string id)
    {
        return $"https://music.163.com/#/song?id={id}";
    }
    private string GetKuGouMusicUrl(string id, string extendData)
    {
        var obj = extendData.ToObject<KuGouSearchExtendData>();
        return $"http://www.kugou.com/song/#hash={obj.Hash}&album_id={obj.AlbumId}&album_audio_id={id}";
    }
    private string GetMiGuMusicUrl(string id)
    {
        return $"http://www.migu.cn/music/detail/{id}.html?migu_p=h5";
    }
    private string GetKuWoMusicUrl(string id)
    {
        return $"http://www.kuwo.cn/play_detail/{id}";
    }
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
        _playerService.NewMusicAdded -= _playerService_NewMusicAdded;
        _timerLyricsUpdate.Tick -= _timerLyricsUpdate_Tick;
        _timerLyricsUpdate.Stop();
    }
}
