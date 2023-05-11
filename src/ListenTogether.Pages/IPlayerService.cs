using ListenTogether.Model;

namespace ListenTogether.Pages;
public interface IPlayerService
{
    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool IsPlaying { get; set; }
    public MusicMetadata? CurrentMetadata { get; set; }
    public MusicPosition CurrentPosition { get; set; }

    public event EventHandler? NewMusicAdded;
    public event EventHandler? IsPlayingChanged;
    public event EventHandler? PositionChanged;
    public event EventHandler? PlayFinished;
    public event EventHandler? PlayFailed;
    public event EventHandler? PlayNext;
    public event EventHandler? PlayPrevious;

    Task PlayAsync(MusicMetadata metadata);
    Task PlayAsync(MusicMetadata metadata, bool isPlaying, double position = 0);
    Task SetPlayPosition(double positionMillisecond);
    Task SetMuted(bool value);
    Task SetVolume(int value);
}