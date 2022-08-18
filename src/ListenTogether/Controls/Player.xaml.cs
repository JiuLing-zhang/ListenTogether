using ListenTogether.Model.Enums;

namespace ListenTogether.Controls;

//TODO 发现一个bug:播放组件静音后，切换页面，静音取消
public partial class Player : ContentView
{
    public static readonly BindableProperty IsMiniWhenPhoneProperty =
        BindableProperty.Create(
            nameof(IsMiniWhenPhone),
            typeof(bool),
            typeof(Player),
            false);
    public bool IsMiniWhenPhone
    {
        get { return (bool)GetValue(IsMiniWhenPhoneProperty); }
        set { SetValue(IsMiniWhenPhoneProperty, value); }
    }

    private PlayerService _playerService;
    private IEnvironmentConfigService _configService;
    public Player()
    {
        InitializeComponent();
        this.IsVisible = false;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (_playerService == null)
        {
            _playerService = this.Handler.MauiContext.Services.GetService<PlayerService>();
            InitPlayer();
        }
        if (_configService == null)
        {
            _configService = this.Handler.MauiContext.Services.GetService<IEnvironmentConfigService>();
        }

        if (Config.Desktop)
        {
            MainBlock.HeightRequest = 90;
        }
        else
        {
            if (IsMiniWhenPhone)
            {
                MainBlock.HeightRequest = 40;
            }
            else
            {
                MainBlock.HeightRequest = 110;
            }
            PhoneMiniPlayer.IsVisible = IsMiniWhenPhone;
            PhoneFullPlayer.IsVisible = !IsMiniWhenPhone;
        }
    }

    internal void OnAppearing()
    {
        InitPlayer();
    }

    void InitPlayer()
    {
        if (_playerService == null)
        {
            return;
        }

        _playerService.IsPlayingChanged += PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded += playerService_NewMusicAdded;
        _playerService.PositionChanged += _playerService_PositionChanged;
        _playerService.BufferingStarted += _playerService_BufferingStarted;
        _playerService.BufferingEnded += _playerService_BufferingEnded;


        UpdateCurrentMusic();
        UpdateRepeatModel();
        if (Config.Desktop)
        {
            UpdateSoundOnOff();
            Updatevolume();
        }
    }
    private void _playerService_BufferingStarted(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.IsVisible = true;
            PlayerLoading.IsVisible = true;
        });
    }

    private void _playerService_BufferingEnded(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            PlayerLoading.IsVisible = false;
        });
    }
    private void PlayerService_IsPlayingChanged(object sender, EventArgs e)
    {
        IsPlayingChangedDo(_playerService.IsPlaying);
    }
    private void IsPlayingChangedDo(bool isPlaying)
    {
        string playImagePath;
        if (!Config.Desktop && IsMiniWhenPhone && !Config.IsDarkTheme)
        {
            playImagePath = isPlaying ? "pause.png" : "play.png";
        }
        else
        {
            playImagePath = isPlaying ? "pause_dark.png" : "play_dark.png";
        }
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.IsVisible = true;
            PlayerLoading.IsVisible = false;
            ImgPlay.Source = playImagePath;
        });
    }

    private void playerService_NewMusicAdded(object sender, EventArgs e)
    {
        NewMusicAddedDo(_playerService.CurrentMusic);
        if (Config.Desktop)
        {
            //TODO 首次播放音乐时，无法设置声音，所以先在这里临时实现
            Updatevolume();
        }
    }

    private void NewMusicAddedDo(Music music)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ImgCurrentMusic.Source = ImageSource.FromStream(
                () => new MemoryStream(ImageCacheUtils.GetByteArrayUsingCache(music.ImageUrl))
            );

            LblMusicName.Text = music.Name;
            LblMusicArtistAndAlbum.Text = $"{music.Artist} - {music.Album}";
            LblMusicArtist.Text = music.Artist;
        });
    }

    private void _playerService_PositionChanged(object sender, EventArgs e)
    {
        PositionChangedDo(_playerService.CurrentPosition);
    }

    private void PositionChangedDo(MusicPosition position)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LblPosition.Text = $"{position.position.Minutes:D2}:{position.position.Seconds:D2}";
            LblDuration.Text = $"{position.Duration.Minutes:D2}:{position.Duration.Seconds:D2}";

            if (!_isPlayProgressDragging)
            {
                SliderPlayProgress.Value = position.PlayProgress;
            }
        });
    }

    private async void ImgPlay_Tapped(object sender, EventArgs e)
    {
        if (_playerService.IsPlaying)
        {
            await _playerService.PauseAsync();
        }
        else
        {
            await _playerService.PlayOnlyAsync();
        }
    }

    private async void ImgSoundOff_Tapped(object sender, EventArgs e)
    {
        SetSoundOnOff();
        UpdateSoundOnOff();
        await WritePlayerSettingAsync();
    }

    private void UpdateCurrentMusic()
    {
        if (_playerService.CurrentMusic == null)
        {
            return;
        }



        var music = new Music()
        {
            Name = _playerService.CurrentMusic.Name,
            Artist = _playerService.CurrentMusic.Artist,
            Album = _playerService.CurrentMusic.Album,
            ImageUrl = _playerService.CurrentMusic.ImageUrl
        };
        NewMusicAddedDo(music);
        PositionChangedDo(_playerService.CurrentPosition);

        IsPlayingChangedDo(_playerService.IsPlaying);
    }

    private void SetSoundOnOff()
    {
        GlobalConfig.MyUserSetting.Player.IsSoundOff = !GlobalConfig.MyUserSetting.Player.IsSoundOff;
    }
    private void UpdateSoundOnOff()
    {
        ImgSoundOff.Source = GlobalConfig.MyUserSetting.Player.IsSoundOff ? "sound_off_dark.png" : "sound_on_dark.png";
        _playerService.SetMuted(GlobalConfig.MyUserSetting.Player.IsSoundOff);
    }
    private void Updatevolume()
    {
        SliderVolume.Value = GlobalConfig.MyUserSetting.Player.Volume;
        _playerService.SetVolume((int)GlobalConfig.MyUserSetting.Player.Volume);
    }

    private async void ImgRepeat_Tapped(object sender, EventArgs e)
    {
        SetNextRepeatMode();
        UpdateRepeatModel();
        await WritePlayerSettingAsync();
    }

    private void SetNextRepeatMode()
    {
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatOne)
        {
            GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.RepeatList;
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.Shuffle;
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            GlobalConfig.MyUserSetting.Player.PlayMode = PlayModeEnum.RepeatOne;
            return;
        }
    }
    private void UpdateRepeatModel()
    {
        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatOne)
        {
            ImgRepeat.Source = "repeat_one_dark.png";
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            ImgRepeat.Source = "repeat_list_dark.png";
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            ImgRepeat.Source = "shuffle_dark.png";
            return;
        }
    }

    private async Task WritePlayerSettingAsync()
    {
        if (_configService == null)
        {
            return;
        }
        await _configService.WritePlayerSettingAsync(GlobalConfig.MyUserSetting.Player);
    }

    private async void Previous_Tapped(object sender, EventArgs e)
    {
        await _playerService.Previous();
    }

    private async void Next_Tapped(object sender, EventArgs e)
    {
        await _playerService.Next();
    }

    private async void Puzzled_Tapped(object sender, EventArgs e)
    {
        await ToastService.Show("别点了，小的就是个占位的~~~");
    }

    private async void SliderVolume_ValueChanged(object sender, ValueChangedEventArgs e)
    {

        int volume = (int)e.NewValue;
        await _playerService.SetVolume(volume);
        GlobalConfig.MyUserSetting.Player.Volume = volume;

        if (GlobalConfig.MyUserSetting.Player.IsSoundOff)
        {
            SetSoundOnOff();
            UpdateSoundOnOff();
        }

        await WritePlayerSettingAsync();
    }

    private bool _isPlayProgressDragging;
    private void SliderPlayProgress_DragStarted(object sender, EventArgs e)
    {
        _isPlayProgressDragging = true;
    }
    private async void SliderPlayProgress_DragCompleted(object sender, EventArgs e)
    {
        if (_playerService.CurrentMusic != null)
        {
            var sliderPlayProgress = sender as Slider;
            var positionMillisecond = _playerService.CurrentPosition.Duration.TotalMilliseconds * sliderPlayProgress.Value;
            await _playerService.SetPlayPosition(positionMillisecond);
        }
        _isPlayProgressDragging = false;
    }

    internal void OnDisappearing()
    {
        _playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded -= playerService_NewMusicAdded;
        _playerService.PositionChanged -= _playerService_PositionChanged;
        _playerService.BufferingStarted -= _playerService_BufferingStarted;
        _playerService.BufferingEnded -= _playerService_BufferingEnded;
    }

    private async void GoToPlayingPage_Tapped(object sender, EventArgs e)
    {
        if (Shell.Current.CurrentPage.GetType() == typeof(PlayingPage))
        {
            return;
        }
        await Shell.Current.GoToAsync($"{nameof(PlayingPage)}", true);
    }
}