namespace MusicPlayerOnline.Maui.Controls;

public partial class Player : ContentView
{
    private PlayerService _playerService;
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
    }

    internal void OnAppearing()
    {
        InitPlayer();
    }

    internal void OnDisappearing()
    {
        _playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
        _playerService.NewMusicAdded -= playerService_NewMusicAdded;
        if (Config.Desktop)
        {
            _playerService.IsVolumeChanged -= playerService_IsVolumeChanged;
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
        if (Config.Desktop)
        {
            _playerService.IsVolumeChanged += playerService_IsVolumeChanged;
        }

        this.IsVisible = _playerService.CurrentMusic != null;
        if (_playerService.CurrentMusic != null)
        {
            UpdatePlayPause();
            UpdateMusicInfo();
        }
    }

    private void playerService_IsVolumeChanged(object sender, EventArgs e)
    {
        BtnSoundOff.Source = _playerService.IsMuted ? "sound_off.png" : "sound_on.png";
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

    private void BtnSoundOff_Clicked(object sender, EventArgs e)
    {

    }


}