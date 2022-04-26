using Android.Media;

namespace ListenTogether.Platforms.Android;
public class AudioService : IAudioService
{
    private MediaPlayer _player;

    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;
    public bool IsPlaying => _player?.IsPlaying ?? false;

    public bool IsMuted { set => throw new NotImplementedException("当前平台不支持此功能（IsMuted）"); }
    public double Volume { set => throw new NotImplementedException("当前平台不支持此功能（Volume）"); }

    public double PositionMillisecond => _player?.CurrentPosition ?? 0;
    public double DurationMillisecond => _player?.Duration ?? 0;

    public async Task InitializeAsync(string uri)
    {
        if (_player == null)
        {
            _player = new MediaPlayer();
            _player.Completion += PlayFinished; ;
            _player.Error += (sender, _) => { PlayFailed?.Invoke(sender, null); };
        }
        else
        {
            if (_player.IsPlaying)
            {
                _player.Stop();
            }
            _player.Reset();
        }

        await _player.SetDataSourceAsync(uri);
        _player.PrepareAsync();
    }

    public Task PlayAsync(double positionMillisecond = 0)
    {
        _player.Start();
        _player.SeekTo((int)positionMillisecond);
        return Task.CompletedTask;
    }

    public Task PauseAsync()
    {
        if (IsPlaying)
        {
            _player.Pause();
        }
        return Task.CompletedTask;
    }
}