using AVFoundation;
using Foundation;

namespace NativeMediaMauiLib.Platforms.iOS;

public class NativeAudioService : INativeAudioService
{
    AVPlayer avPlayer;
    string _uri;

    public bool IsPlaying => avPlayer != null
        ? avPlayer.TimeControlStatus == AVPlayerTimeControlStatus.Playing
        : false;

    public double CurrentPositionMillisecond => avPlayer?.CurrentTime.Seconds * 1000 ?? 0;
    public double CurrentDurationMillisecond => avPlayer?.CurrentItem?.Asset.Duration.Seconds * 1000 ?? 0;
    public event EventHandler<bool> IsPlayingChanged;
    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;
    public event EventHandler Played;
    public event EventHandler Paused;
    public event EventHandler Stopped;
    public event EventHandler SkipToNext;
    public event EventHandler SkipToPrevious;

    public async Task InitializeAsync(string audioURI, AudioMetadata audioMetadata)
    {
        _uri = audioURI;
        NSUrl fileURL = new NSUrl(_uri.ToString());

        if (avPlayer != null)
        {
            await PauseAsync();
        }

        avPlayer = new AVPlayer(fileURL);
    }

    public Task PauseAsync()
    {
        avPlayer?.Pause();

        return Task.CompletedTask;
    }

    public async Task PlayAsync(double positionMillisecond = 0)
    {
        await avPlayer.SeekAsync(new CoreMedia.CMTime(((long)positionMillisecond / 1000), 1));
        avPlayer?.Play();
    }

    public Task SetCurrentTime(double value)
    {
        return avPlayer.SeekAsync(new CoreMedia.CMTime((long)value, 1));
    }

    public Task SetMuted(bool value)
    {
        if (avPlayer != null)
        {
            avPlayer.Muted = value;
        }

        return Task.CompletedTask;
    }

    public Task SetVolume(int value)
    {
        if (avPlayer != null)
        {
            avPlayer.Volume = value;
        }

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        avPlayer?.Dispose();
        return ValueTask.CompletedTask;
    }
}
