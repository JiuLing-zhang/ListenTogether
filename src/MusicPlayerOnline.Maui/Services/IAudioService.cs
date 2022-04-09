namespace MusicPlayerOnline.Maui.Services;

public interface IAudioService
{
    Task InitializeAsync(string uri);
    Task PlayAsync(double position = 0);
    Task PauseAsync();
    bool IsPlaying { get; }
    double CurrentPosition { get; }

    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;
}