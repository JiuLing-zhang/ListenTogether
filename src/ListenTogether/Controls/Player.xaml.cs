using ListenTogether.Model.Enums;

namespace ListenTogether.Controls;

public partial class Player : ContentView
{
    public static readonly BindableProperty IsPlayingPageProperty =
        BindableProperty.Create(
            nameof(IsPlayingPage),
            typeof(bool),
            typeof(Player),
            false);
    public bool IsPlayingPage
    {
        get { return (bool)GetValue(IsPlayingPageProperty); }
        set { SetValue(IsPlayingPageProperty, value); }
    }

    private PlayerService _playerService = null!;
    private IEnvironmentConfigService _configService = null!;
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
            _playerService = this.Handler.MauiContext.Services.GetRequiredService<PlayerService>();
            InitPlayer();
        }
        if (_configService == null)
        {
            _configService = this.Handler.MauiContext.Services.GetRequiredService<IEnvironmentConfigService>();
        }

        if (Config.Desktop)
        {
            MainBlock.HeightRequest = 90;
        }
        else
        {
            MainBlock.HeightRequest = IsPlayingPage ? 110 : 40;
            PhoneMiniPlayer.IsVisible = !IsPlayingPage;
            PhoneFullPlayer.IsVisible = IsPlayingPage;
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

        UpdateCurrentMusic();
        UpdateRepeatModel();
        if (Config.Desktop)
        {
            UpdateSoundOnOff().Wait();
            UpdateVolume().Wait();
        }
        if (!IsPlayingPage && !Config.IsDarkTheme)
        {
            ImgBack.Source = "back.png";
            ImgNext.Source = "next.png";
            ImgOther.Source = "puzzled.png";

            //TODO 目前 MAUI 无法正确读取资源文件，所以临时使用16进制颜色解决
            //var lightText = (Color)Application.Current.Resources["LightText"];
            //var lightTextSecond = (Color)Application.Current.Resources["LightTextSecond"];            
            LblMusicInfo.TextColor = Color.FromArgb("#262626");

            SliderPlayProgress.MinimumTrackColor = Color.FromArgb("#262626");
            SliderPlayProgress.MaximumTrackColor = Color.FromArgb("#717171");
            SliderPlayProgress.ThumbColor = Color.FromArgb("#C98FFF");
        }
        else
        {
            ImgBack.Source = "back_dark.png";
            ImgNext.Source = "next_dark.png";
            ImgOther.Source = "puzzled_dark.png";

            //TODO 目前 MAUI 无法正确读取资源文件，所以临时使用16进制颜色解决
            //var darkText = (Color)Application.Current.Resources["DarkText"]; 
            //var darkTextSecond = (Color)Application.Current.Resources["DarkTextSecond"];
            LblMusicInfo.TextColor = Color.FromArgb("#FCF2F7");

            SliderPlayProgress.MinimumTrackColor = Color.FromArgb("#FFFFFF");
            SliderPlayProgress.MaximumTrackColor = Color.FromArgb("#FCF2F7");
            SliderPlayProgress.ThumbColor = Color.FromArgb("#C98FFF");
        }
    }

    private void PlayerService_IsPlayingChanged(object? sender, EventArgs e)
    {
        IsPlayingChangedDo(_playerService.IsPlaying);
    }
    private void IsPlayingChangedDo(bool isPlaying)
    {
        string playImagePath;
        if (!IsPlayingPage && !Config.IsDarkTheme)
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
            ImgPlay.Source = playImagePath;
        });
    }

    private void playerService_NewMusicAdded(object? sender, EventArgs e)
    {
        NewMusicAddedDo(_playerService.CurrentMusic);
    }

    private void NewMusicAddedDo(Music music)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ImgCurrentMusic.Source = ImageSource.FromStream(
                () => new MemoryStream(ImageCacheUtils.GetByteArrayUsingCache(music.ImageUrl))
            );

            LblMusicInfo.Text = $"{music.Name} - {music.Artist}";
        });
    }

    private void _playerService_PositionChanged(object? sender, EventArgs e)
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
        await _playerService.PlayAsync(_playerService.CurrentMusic);
    }

    private async void ImgSoundOff_Tapped(object sender, EventArgs e)
    {
        GlobalConfig.MyUserSetting.Player.IsSoundOff = !GlobalConfig.MyUserSetting.Player.IsSoundOff;
        await WritePlayerSettingAsync();
        await UpdateSoundOnOff();
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

    private async Task UpdateSoundOnOff()
    {
        string imagePath;
        if (!IsPlayingPage && !Config.IsDarkTheme)
        {
            imagePath = GlobalConfig.MyUserSetting.Player.IsSoundOff ? "sound_off.png" : "sound_on.png";
        }
        else
        {
            imagePath = GlobalConfig.MyUserSetting.Player.IsSoundOff ? "sound_off_dark.png" : "sound_on_dark.png";
        }

        ImgSoundOff.Source = imagePath;
        await _playerService.SetMuted(GlobalConfig.MyUserSetting.Player.IsSoundOff);
    }
    private async Task UpdateVolume()
    {
        SliderVolume.Value = GlobalConfig.MyUserSetting.Player.Volume;
        await _playerService.SetVolume((int)GlobalConfig.MyUserSetting.Player.Volume);
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
            if (!IsPlayingPage && !Config.IsDarkTheme)
            {
                ImgRepeat.Source = "repeat_one.png";
            }
            else
            {
                ImgRepeat.Source = "repeat_one_dark.png";
            }
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            if (!IsPlayingPage && !Config.IsDarkTheme)
            {
                ImgRepeat.Source = "repeat_list.png";
            }
            else
            {
                ImgRepeat.Source = "repeat_list_dark.png";
            }
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            if (!IsPlayingPage && !Config.IsDarkTheme)
            {
                ImgRepeat.Source = "shuffle.png";
            }
            else
            {
                ImgRepeat.Source = "shuffle_dark.png";
            }
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
            if (sliderPlayProgress != null)
            {
                var positionMillisecond = _playerService.CurrentPosition.Duration.TotalMilliseconds * sliderPlayProgress.Value;
                await _playerService.SetPlayPosition(positionMillisecond);
            }
        }
        _isPlayProgressDragging = false;
    }

    internal void OnDisappearing()
    {
        _playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded -= playerService_NewMusicAdded;
        _playerService.PositionChanged -= _playerService_PositionChanged;
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