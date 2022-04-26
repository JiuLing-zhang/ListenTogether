using Windows.Media.Core;
using Windows.Media.Playback;

namespace MusicPlayerOnline.Maui.Platforms.Windows;
public class AudioService : IAudioService
{
    string _uri;
    MediaPlayer _mediaPlayer;
    public AudioService()
    {
        _mediaPlayer = new MediaPlayer
        {
            AudioCategory = MediaPlayerAudioCategory.Media
        };
        _mediaPlayer.MediaEnded += (_, _) => PlayFinished?.Invoke(null, null);
        _mediaPlayer.MediaFailed += (_, _) => PlayFailed?.Invoke(null, null);
    }

    public bool IsPlaying => _mediaPlayer.CurrentState == MediaPlayerState.Playing;
    public double PositionMillisecond => (long)_mediaPlayer.Position.TotalMilliseconds;
    public double DurationMillisecond => (long)_mediaPlayer.NaturalDuration.TotalMilliseconds;


    public bool IsMuted { set => _mediaPlayer.IsMuted = value; }
    public double Volume { set => _mediaPlayer.Volume = value; }

    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;

    public async Task InitializeAsync(string audioURI)
    {
        _uri = audioURI;
        if (_mediaPlayer != null)
        {
            await PauseAsync();
            _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(_uri));
        }
    }

    public Task PauseAsync()
    {
        _mediaPlayer?.Pause();
        return Task.CompletedTask;
    }

    public Task PlayAsync(double positionMillisecond = 0)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Position = TimeSpan.FromMilliseconds(positionMillisecond);
            _mediaPlayer.Play();
        }

        return Task.CompletedTask;
    }
}