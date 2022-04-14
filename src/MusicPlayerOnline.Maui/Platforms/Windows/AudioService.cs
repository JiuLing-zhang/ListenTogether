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
        _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        _mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
    }

    public bool IsPlaying => _mediaPlayer.CurrentState == MediaPlayerState.Playing;
    public double CurrentPosition => (long)_mediaPlayer.Position.TotalSeconds;
    public double Duration => (long)_mediaPlayer.NaturalDuration.TotalSeconds;


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

    private void MediaPlayer_MediaEnded(object s, object s1)
    {
        PlayFinished?.Invoke(null, null);
    }
    private void MediaPlayer_MediaFailed(object s, object s1)
    {
        PlayFailed?.Invoke(null, null);
    }

    public Task PauseAsync()
    {
        _mediaPlayer?.Pause();
        return Task.CompletedTask;
    }

    public Task PlayAsync(double position = 0)
    {
        if (_mediaPlayer != null)
        {
            _mediaPlayer.Position = TimeSpan.FromSeconds(position);
            _mediaPlayer.Play();
        }

        return Task.CompletedTask;
    }
}