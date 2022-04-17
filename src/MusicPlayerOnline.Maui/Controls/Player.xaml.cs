using Microsoft.Maui.Dispatching;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Maui.Controls;

public partial class Player : ContentView
{
    private PlayerService _playerService;
    private IDispatcherTimer? _timerPlayProgress;
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

        if (_timerPlayProgress == null)
        {
            _timerPlayProgress = App.Current.Dispatcher.CreateTimer();
            _timerPlayProgress.Interval = TimeSpan.FromMilliseconds(500);
            _timerPlayProgress.Tick += _timerPlayProgress_Tick;
            _timerPlayProgress.Start();
        }
    }

    private void _timerPlayProgress_Tick(object sender, EventArgs e)
    {
        if (_playerService == null)
        {
            return;
        }
        try
        {
            var positionMillisecond = _playerService.PositionMillisecond;

            var tsPosition = TimeSpan.FromMilliseconds(positionMillisecond);
            LblPositionMilliseconds.Text = $"{tsPosition.Minutes:D2}:{tsPosition.Seconds:D2}";

            double PlayProgress = positionMillisecond / _playerService.DurationMillisecond;
            SliderPlayProgress.Value = PlayProgress;
        }
        catch (Exception ex)
        {
            Logger.Error("播放组件更新失败。", ex);
        }
    }

    internal void OnAppearing()
    {
        InitPlayer();

        if (_timerPlayProgress != null)
        {
            _timerPlayProgress.Start();
        }
    }


    internal void OnDisappearing()
    {
        _playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded -= playerService_NewMusicAdded;
        if (_timerPlayProgress != null)
        {
            _timerPlayProgress.Stop();
        }
    }

    void InitPlayer()
    {
        if (_playerService == null)
        {
            return;
        }

        _playerService.IsPlayingChanged += PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded += playerService_NewMusicAdded;

        this.IsVisible = _playerService.CurrentMusic != null;
        if (_playerService.CurrentMusic != null)
        {
            UpdatePlayPause();
            UpdateMusicInfo();
        }
        UpdateSoundOnOff();
        Updatevolume();
        UpdateRepeatModel();
    }

    private void PlayerService_IsPlayingChanged(object sender, EventArgs e)
    {
        if (_playerService.CurrentMusic == null)
        {
            IsVisible = false;
        }
        else
        {
            if (Dispatcher.IsDispatchRequired)
            {
                Dispatcher.Dispatch(() =>
                {
                    UpdatePlayPause();
                });
            }
            else
            {
                UpdatePlayPause();
            }
        }
    }
    private void playerService_NewMusicAdded(object sender, EventArgs e)
    {
        if (Dispatcher.IsDispatchRequired)
        {
            Dispatcher.Dispatch(() =>
            {
                UpdateMusicInfo();
            });
        }
        else
        {
            UpdateMusicInfo();
        }
    }

    private void UpdatePlayPause()
    {
        this.IsVisible = true;
        ImgPlay.Source = _playerService.IsPlaying ? "pause.png" : "play.png";
    }
    private void UpdateMusicInfo()
    {
        ImgCurrentMusic.Source = _playerService.CurrentMusic.ImageUrl;
        LblName.Text = _playerService.CurrentMusic.Name;
        LblAuthor.Text = $"{_playerService.CurrentMusic.Artist} - {_playerService.CurrentMusic.Album}";

        var tsDuration = TimeSpan.FromMilliseconds(_playerService.DurationMillisecond);
        LblDurationMilliseconds.Text = $"{tsDuration.Minutes:D2}:{tsDuration.Seconds:D2}";
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

    private void ImgSoundOff_Tapped(object sender, EventArgs e)
    {
        SetSoundOnOff();
        UpdateSoundOnOff();
        WritePlayerSetting();
    }
    private void SetSoundOnOff()
    {
        GlobalConfig.MyUserSetting.Player.IsSoundOff = !GlobalConfig.MyUserSetting.Player.IsSoundOff;
    }
    private void UpdateSoundOnOff()
    {
        ImgSoundOff.Source = GlobalConfig.MyUserSetting.Player.IsSoundOff ? "sound_off.png" : "sound_on.png";
        _playerService.IsMuted = GlobalConfig.MyUserSetting.Player.IsSoundOff;
    }
    private void Updatevolume()
    {
        SliderVolume.Value = GlobalConfig.MyUserSetting.Player.Volume;
    }

    private void ImgRepeat_Tapped(object sender, EventArgs e)
    {
        SetNextRepeatMode();
        UpdateRepeatModel();
        WritePlayerSetting();
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
            ImgRepeat.Source = "repeat_one.png";
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.RepeatList)
        {
            ImgRepeat.Source = "repeat_list.png";
            return;
        }

        if (GlobalConfig.MyUserSetting.Player.PlayMode == PlayModeEnum.Shuffle)
        {
            ImgRepeat.Source = "shuffle.png";
            return;
        }
    }

    private void WritePlayerSetting()
    {
        if (_configService == null)
        {
            return;
        }
        _configService.WritePlayerSetting(GlobalConfig.MyUserSetting.Player);
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

    private void SliderVolume_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        _playerService.Volume = e.NewValue;
        GlobalConfig.MyUserSetting.Player.Volume = e.NewValue;

        if (GlobalConfig.MyUserSetting.Player.IsSoundOff)
        {
            SetSoundOnOff();
            UpdateSoundOnOff();
        }

        WritePlayerSetting();
    }

    private void SliderPlayProgress_DragStarted(object sender, EventArgs e)
    {
        _timerPlayProgress?.Stop();
    }

    private void SliderPlayProgress_DragCompleted(object sender, EventArgs e)
    {
        if (_playerService.CurrentMusic != null)
        {
            var positionMillisecond = _playerService.DurationMillisecond * SliderPlayProgress.Value;
            _playerService.PlayAsync(_playerService.CurrentMusic, positionMillisecond);
        }
        _timerPlayProgress?.Start();
    }
}