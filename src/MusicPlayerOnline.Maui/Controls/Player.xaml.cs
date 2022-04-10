using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Maui.Controls;

public partial class Player : ContentView
{
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
    }

    internal void OnAppearing()
    {
        InitPlayer();
    }

    internal void OnDisappearing()
    {
        _playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded -= playerService_NewMusicAdded;
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
        UpdateRepeatModel();
    }

    private void playerService_IsVolumeChanged(object sender, EventArgs e)
    {
        UpdateSoundOnOff();
    }

    private void PlayerService_IsPlayingChanged(object sender, EventArgs e)
    {
        if (_playerService.CurrentMusic == null)
        {
            IsVisible = false;
        }
        else
        {
            UpdatePlayPause();
        }
    }
    private void playerService_NewMusicAdded(object sender, EventArgs e)
    {
        UpdateMusicInfo();
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
        LblDuration.Text = _playerService.CurrentMusic.Duration.ToString();
    }



    private async void ImgPlay_Tapped(object sender, EventArgs e)
    {
        await _playerService.PlayAsync(_playerService.CurrentMusic);
    }

    private void ImgSoundOff_Tapped(object sender, EventArgs e)
    {
        SetSoundOnOff();
        UpdateSoundOnOff();
        WritePlayerSett();
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

    private void ImgRepeat_Tapped(object sender, EventArgs e)
    {
        SetNextRepeatMode();
        UpdateRepeatModel();
        WritePlayerSett();
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

    private void WritePlayerSett()
    {
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

    private void Puzzled_Tapped(object sender, EventArgs e)
    {
        ToastService.Show("别点了，小的就是个占位的~~~");
    }

    private async void GotoPlaying_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(PlayingPage)}");
    }
}