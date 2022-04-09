using Windows.Media.Core;
using Windows.Media.Playback;

namespace MusicPlayerOnline.Maui.Platforms.Windows;
public class AudioService : IAudioService
{
    string _uri;
    MediaPlayer _mediaPlayer;

    public bool IsPlaying => _mediaPlayer != null && _mediaPlayer.CurrentState == MediaPlayerState.Playing;

    public double CurrentPosition => (long)_mediaPlayer?.Position.TotalSeconds;

    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;

    public async Task InitializeAsync(string audioURI)
    {
        _uri = audioURI;

        if (_mediaPlayer == null)
        {
            _mediaPlayer = new MediaPlayer
            {
                Source = MediaSource.CreateFromUri(new Uri(_uri)),
                AudioCategory = MediaPlayerAudioCategory.Media
            };
            _mediaPlayer.MediaEnded += T;
            _mediaPlayer.MediaFailed += T2;
        }
        if (_mediaPlayer != null)
        {
            await PauseAsync();
            _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(_uri));
        }
    }

    private void T(object s, object s1)
    {
        PlayFinished?.Invoke(null, null);
    }
    private void T2(object s, object s1)
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