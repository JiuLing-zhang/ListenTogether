namespace MusicPlayerOnline.Maui.Services;

public interface IAudioService
{
    Task InitializeAsync(string uri);
    Task PlayAsync(double position = 0);
    Task PauseAsync();

    /// <summary>
    /// 是否静音
    /// </summary>
    bool IsMuted { get; set; }
    /// <summary>
    /// 声音大小
    /// </summary>
    double VoiceValue { get; set; }

    bool IsPlaying { get; }
    double CurrentVolume { get; }
    double CurrentPosition { get; }

    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;
}