using ListenTogether.Model.Enums;

namespace ListenTogether.Controls;

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
            _playerService.IsPlayingChanged += PlayerService_IsPlayingChanged;
            _playerService.NewMusicAdded += playerService_NewMusicAdded;
            _playerService.PositionChanged += _playerService_PositionChanged;
            InitPlayer();
        }
        if (_configService == null)
        {
            _configService = this.Handler.MauiContext.Services.GetService<IEnvironmentConfigService>();
        }

        PhoneMiniBlock.IsVisible = IsMiniWhenPhone;
        PhoneBlock.IsVisible = !IsMiniWhenPhone;
        if (Config.Desktop)
        {
            MainBlock.HeightRequest = 70;
        }
        else
        {
            if (IsMiniWhenPhone)
            {
                MainBlock.HeightRequest = 30;
            }
            else
            {
                MainBlock.HeightRequest = 60;
            }
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

        UpdateCurrentMusic();
        UpdateRepeatModel();
        if (Config.Desktop)
        {
            UpdateSoundOnOff();
            Updatevolume();
        }
    }

    private void PlayerService_IsPlayingChanged(object sender, bool e)
    {
        IsPlayingChangedDo(e);
    }
    private void IsPlayingChangedDo(bool isPlaying)
    {
        string playImagePath = isPlaying ? GetImageName("pause") : GetImageName("play");
        if (Dispatcher.IsDispatchRequired)
        {
            Dispatcher.Dispatch(() =>
            {
                ImgPlay.Source = playImagePath;
            });
        }
        else
        {
            ImgPlay.Source = playImagePath;
        }
    }

    private void playerService_NewMusicAdded(object sender, Music e)
    {
        this.IsVisible = true;
        NewMusicAddedDo(e);
    }

    private void NewMusicAddedDo(Music music)
    {
        if (Dispatcher.IsDispatchRequired)
        {
            Dispatcher.Dispatch(() =>
            {
                ImgCurrentMusic.Source = music.ImageUrl;
                LblMusicName.Text = music.Name;
                LblMusicInfo.Text = $"{music.Artist} - {music.Album}";
                LblMusicArtist.Text = music.Artist;
            });
        }
        else
        {
            ImgCurrentMusic.Source = music.ImageUrl;
            LblMusicName.Text = music.Name;
            LblMusicInfo.Text = $"{music.Artist} - {music.Album}";
            LblMusicArtist.Text = music.Artist;
        }
    }

    private void _playerService_PositionChanged(object sender, MusicPosition e)
    {
        PositionChangedDo(e);
    }

    private void PositionChangedDo(MusicPosition position)
    {
        if (Dispatcher.IsDispatchRequired)
        {
            Dispatcher.Dispatch(() =>
            {
                LblPositionMilliseconds.Text = $"{position.position.Minutes:D2}:{position.position.Seconds:D2}";
                LblDurationMilliseconds.Text = $"{position.Duration.Minutes:D2}:{position.Duration.Seconds:D2}";

                if (!_isPlayProgressDragging)
                {
                    SliderPlayProgress.Value = position.PlayProgress;
                }
            });
        }
        else
        {
            LblPositionMilliseconds.Text = $"{position.position.Minutes:D2}:{position.position.Seconds:D2}";
            LblDurationMilliseconds.Text = $"{position.Duration.Minutes:D2}:{position.Duration.Seconds:D2}";

            if (!_isPlayProgressDragging)
            {
                SliderPlayProgress.Value = position.PlayProgress;
            }
        }
    }

    private async void ImgPlay_Tapped(object sender, EventArgs e)
    {
        if (_playerService.IsPlaying)
        {
            await _playerService.PauseAsync();
        }
        else
        {
            await _playerService.PlayAsync(_playerService.CurrentMusic, _playerService.PositionMillisecond);
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
        this.IsVisible = true;

        var music = new Music()
        {
            Name = _playerService.CurrentMusic.Name,
            Artist = _playerService.CurrentMusic.Artist,
            Album = _playerService.CurrentMusic.Album,
            ImageUrl = _playerService.CurrentMusic.ImageUrl
        };
        NewMusicAddedDo(music);

        var position = new MusicPosition()
        {
            position = TimeSpan.FromMilliseconds(_playerService.PositionMillisecond),
            Duration = TimeSpan.FromMilliseconds(_playerService.DurationMillisecond),
            PlayProgress = _playerService.PositionMillisecond / _playerService.DurationMillisecond
        };
        PositionChangedDo(position);

        IsPlayingChangedDo(_playerService.IsPlaying);
    }

    private void SetSoundOnOff()
    {
        GlobalConfig.MyUserSetting.Player.IsSoundOff = !GlobalConfig.MyUserSetting.Player.IsSoundOff;
    }
    private void UpdateSoundOnOff()
    {
        ImgSoundOff.Source = GlobalConfig.MyUserSetting.Player.IsSoundOff ? GetImageName("sound_off") : GetImageName("sound_on");
        _playerService.IsMuted = GlobalConfig.MyUserSetting.Player.IsSoundOff;
    }
    private void Updatevolume()
    {
        SliderVolume.Value = GlobalConfig.MyUserSetting.Player.Volume;
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
            ImgRepeat.Source = GetImageName("repeat_one");
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            ImgRepeat.Source = GetImageName("repeat_list");
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            ImgRepeat.Source = GetImageName("shuffle");
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
        _playerService.Volume = e.NewValue;
        GlobalConfig.MyUserSetting.Player.Volume = e.NewValue;

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
            var positionMillisecond = _playerService.DurationMillisecond * sliderPlayProgress.Value;
            await _playerService.PlayAsync(_playerService.CurrentMusic, positionMillisecond);
        }
        _isPlayProgressDragging = false;
    }

    private string GetImageName(string imageName)
    {
        if (Config.IsDarkTheme)
        {
            return $"{imageName}_dark.png";
        }
        return $"{imageName}.png";
    }

    private async void ImgPlaying_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("PlayingPage");
    }
}