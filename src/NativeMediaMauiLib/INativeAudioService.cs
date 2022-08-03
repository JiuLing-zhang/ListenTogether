namespace NativeMediaMauiLib;

//音频播放模块根据 “MAUI播客项目” 修改
//https://github.com/microsoft/dotnet-podcasts/blob/main/src/Lib/SharedMauiLib/INativeAudioService.cs

public interface INativeAudioService
{
    Task InitializeAsync(string audioURI);

    Task PlayAsync(double position = 0);

    Task PauseAsync();

    Task SetMuted(bool value);

    Task SetVolume(int value);

    Task SetCurrentTime(double value);

    ValueTask DisposeAsync();

    bool IsPlaying { get; }

    double CurrentPosition { get; }

    double CurrentDuration { get; }

    event EventHandler<bool> IsPlayingChanged;
    event EventHandler PlayFinished;
    event EventHandler PlayFailed;
}