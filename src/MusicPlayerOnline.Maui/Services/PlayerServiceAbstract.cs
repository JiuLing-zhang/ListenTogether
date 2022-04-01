using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Maui.Services;

internal abstract class PlayerServiceAbstract
{
    public abstract double CurrentPosition { get; }
    public abstract Music CurrentMusic { get; }
    public abstract bool IsPlaying { get; }

    public abstract event EventHandler NewMusicAdded;
    public abstract event EventHandler PlayFinished;

    public abstract Task PlayAsync(Music music, double position = 0);
    public abstract Task PauseAsync();
}