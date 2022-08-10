namespace NativeMediaMauiLib;

//音频播放模块根据 “MAUI播客项目” 修改
//https://github.com/microsoft/dotnet-podcasts/blob/main/src/Lib/SharedMauiLib/INativeAudioService.cs

public interface INativeAudioService
{
    Task InitializeAsync(string audioURI, AudioMetadata audioMetadata);

    Task PlayAsync(double positionMillisecond = 0);

    Task PauseAsync();

    Task SetMuted(bool value);

    Task SetVolume(int value);

    Task SetCurrentTime(double value);

    ValueTask DisposeAsync();

    bool IsPlaying { get; }

    double CurrentPositionMillisecond { get; }

    double CurrentDurationMillisecond { get; }

    event EventHandler<bool> IsPlayingChanged;
    event EventHandler PlayFinished;
    event EventHandler PlayFailed;

    //外部事件（线控、媒体中心按钮回调等）
    event EventHandler Played;
    event EventHandler Paused;
    event EventHandler Stopped;
    event EventHandler SkipToNext;
    event EventHandler SkipToPrevious;
}