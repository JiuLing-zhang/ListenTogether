using System.Collections.ObjectModel;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Maui.ViewModels
{
    public class PlayingPageViewModel : ViewModelBase
    {
        private readonly PlayerService _playerService;
        public Command PlayerStateChangeCommand => new Command(PlayerStateChange);
        public Command RepeatTypeChangeCommand => new Command(RepeatTypeChange);
        public Command PreviousCommand => new Command(Previous);
        public Command NextCommand => new Command(Next);

        public Command TempButtonCommand => new Command(() =>
        {
            ToastService.Show("不知道放啥了，占个位而已~~");
        });

        public Action<LyricViewModel> ScrollLyric { get; set; }

        private IEnvironmentConfigService _configService;
        public PlayingPageViewModel(IEnvironmentConfigService configService, PlayerService playerService)
        {
            _configService = configService;
            _playerService = playerService;

            Lyrics = new ObservableCollection<LyricViewModel>();

            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
                {
                    Device.BeginInvokeOnMainThread(UpdatePlayingProgress);
                    return true;
                }
           );

            //TODO 更新页面
            //PlayerService.Instance().MediaBegin += UpdatePlayingMusicInfo;

        }

        public async Task InitializeAsync()
        {
            UpdateCurrentMusic();
            GetLyricDetail();

            PlayModeInt = (int)GlobalConfig.MyUserSetting.Player.PlayMode;
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "正在播放";

        private bool _isPlaying;
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
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

        private decimal _position;
        /// <summary>
        /// 播放进度
        /// </summary>
        public decimal Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }


        private string _durationTime;
        /// <summary>
        /// 时长时间
        /// </summary>
        public string DurationTime
        {
            get => _durationTime;
            set
            {
                _durationTime = value;
                OnPropertyChanged();
            }
        }

        private string _currentTime;
        /// <summary>
        /// 当前时间
        /// </summary>
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        private int _playModeInt;
        /// <summary>
        /// 播放模式
        /// </summary>
        public int PlayModeInt
        {
            get => _playModeInt;
            set
            {
                _playModeInt = value;
                OnPropertyChanged();
            }
        }

        private void UpdateCurrentMusic()
        {
            CurrentMusic = _playerService.CurrentMusic;
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
                Lyrics.Add(new LyricViewModel() { Position = totalMillisecond, Info = info });
            }
        }

        /// <summary>
        /// 更新播放进度
        /// </summary>
        private void UpdatePlayingProgress()
        {
            //TODO 更新播放进度
            //if (PlayerService.Instance().PlayingMusic == null)
            //{
            //    return;
            //}

            //if (PlayerService.Instance().IsPlaying == false)
            //{
            //    return;
            //}

            //var (duration, position) = PlayerService.Instance().GetPosition();
            //Position = (decimal)position / duration;

            //var tsDurationTime = TimeSpan.FromMilliseconds(duration);
            //DurationTime = $"{tsDurationTime.Minutes:D2}:{tsDurationTime.Seconds:D2}";

            //var tsCurrentTime = TimeSpan.FromMilliseconds(position);
            //CurrentTime = $"{tsCurrentTime.Minutes:D2}:{tsCurrentTime.Seconds:D2}";

            //IsPlaying = PlayerService.Instance().IsPlaying;

            //if (Lyrics.Count > 0)
            //{
            //    //取大于当前进度的第一行索引，在此基础上-1则为需要高亮的行
            //    int highlightIndex = 0;
            //    foreach (var lyric in Lyrics)
            //    {
            //        lyric.IsHighlight = false;
            //        if (lyric.Position > position)
            //        {
            //            break;
            //        }
            //        highlightIndex++;
            //    }
            //    if (highlightIndex > 0)
            //    {
            //        highlightIndex = highlightIndex - 1;
            //    }

            //    Lyrics[highlightIndex].IsHighlight = true;
            //    ScrollLyric.Invoke(Lyrics[highlightIndex]);
            //}
        }
        //暂停、恢复
        private void PlayerStateChange()
        {
            if (CurrentMusic == null)
            {
                ToastService.Show("亲，当前没有歌曲可以播放");
                return;
            }

            //TODO 播放和暂停
            //if (IsPlaying == true)
            //{
            //    PlayerService.Instance().Pause();
            //}
            //else
            //{
            //    PlayerService.Instance().Start();
            //}

            IsPlaying = !IsPlaying;
        }
        //循环方式
        private async void RepeatTypeChange()
        {
            switch (GlobalConfig.MyUserSetting.Player.PlayMode)
            {
                case PlayModeEnum.RepeatOne:
                    GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.RepeatList;
                    break;
                case PlayModeEnum.RepeatList:
                    GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.Shuffle;
                    break;
                case PlayModeEnum.Shuffle:
                    GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.RepeatOne;
                    break;
            }
            PlayModeInt = (int)GlobalConfig.MyUserSetting.Player.PlayMode;
            ToastService.Show($"{GlobalConfig.MyUserSetting.Player.PlayMode.GetDescription()}");

            _configService.WritePlayerSetting(GlobalConfig.MyUserSetting.Player);
        }
        /// <summary>
        /// 上一首
        /// </summary>
        private async void Previous()
        {
            if (CurrentMusic == null)
            {
                ToastService.Show("目前这个歌曲似乎好像有问题哟~");
                return;
            }
            //TODO 上一首
            //await PlayerService.Instance().Previous(); 
        }

        /// <summary>
        /// 下一首
        /// </summary>
        private async void Next()
        {
            if (CurrentMusic == null)
            {
                ToastService.Show("目前这个歌曲似乎好像有问题哟~");
                return;
            }
            //TODO 下一首
            //await PlayerService.Instance().Next(); 
        }
    }
}
