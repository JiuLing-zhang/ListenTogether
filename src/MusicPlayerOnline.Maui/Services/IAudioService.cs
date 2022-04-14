namespace MusicPlayerOnline.Maui.Services;

public interface IAudioService
{
    Task InitializeAsync(string uri);
    Task PlayAsync(double position = 0);
    Task PauseAsync();

    /// <summary>
    /// 是否静音
    /// </summary>
    bool IsMuted { set; }
    /// <summary>
    /// 声音大小
    /// </summary>
    double Volume { set; }

    bool IsPlaying { get; }
    double CurrentPosition { get; }
    double Duration { get; }

    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;
}