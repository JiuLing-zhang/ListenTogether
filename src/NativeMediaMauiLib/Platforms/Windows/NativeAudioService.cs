using Windows.Media.Core;
using Windows.Media.Playback;

namespace NativeMediaMauiLib.Platforms.Windows;

public class NativeAudioService : INativeAudioService
{
    string _uri;
    MediaPlayer mediaPlayer;

    public bool IsPlaying => mediaPlayer != null
        && mediaPlayer.CurrentState == MediaPlayerState.Playing;

    public double CurrentPosition => mediaPlayer?.Position.TotalSeconds ?? 0;
    public double CurrentDuration => mediaPlayer?.NaturalDuration.TotalSeconds ?? 0;
    public event EventHandler<bool> IsPlayingChanged;
    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;

    public async Task InitializeAsync(string audioURI)
    {
        _uri = audioURI;

        if (mediaPlayer == null)
        {
            mediaPlayer = new MediaPlayer
            {
                Source = MediaSource.CreateFromUri(new Uri(_uri)),
                AudioCategory = MediaPlayerAudioCategory.Media
            };
            mediaPlayer.MediaEnded += (_, _) => PlayFinished?.Invoke(this, EventArgs.Empty);
            mediaPlayer.MediaFailed += (_, _) => PlayFailed?.Invoke(this, EventArgs.Empty);
        }
        if (mediaPlayer != null)
        {
            await PauseAsync();
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(_uri));
        }
    }

    public Task PauseAsync()
    {
        mediaPlayer?.Pause();
        return Task.CompletedTask;
    }

    public Task PlayAsync(double position = 0)
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(position);
            mediaPlayer.Play();
        }

        return Task.CompletedTask;
    }

    public Task SetCurrentTime(double value)
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(value);
        }

        return Task.CompletedTask;
    }

    public Task SetMuted(bool value)
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.IsMuted = value;
        }

        return Task.CompletedTask;
    }

    public Task SetVolume(int value)
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Volume = value != 0
                ? value / 100d
                : 0;
        }

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        mediaPlayer?.Dispose();
        return ValueTask.CompletedTask;
    }
}

