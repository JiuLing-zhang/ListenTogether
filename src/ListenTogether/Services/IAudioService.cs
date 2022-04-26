namespace MusicPlayerOnline.Maui.Services;

public interface IAudioService
{
    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;

    /// <summary>
    /// 是否静音
    /// </summary>
    bool IsMuted { set; }
    /// <summary>
    /// 声音大小
    /// </summary>
    double Volume { set; }

    bool IsPlaying { get; }
    double PositionMillisecond { get; }
    double DurationMillisecond { get; }

    Task InitializeAsync(string uri);
    Task PlayAsync(double positionMillisecond = 0);
    Task PauseAsync();
}