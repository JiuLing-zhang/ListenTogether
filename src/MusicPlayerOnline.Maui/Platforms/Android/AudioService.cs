using Android.Media;

namespace MusicPlayerOnline.Maui.Platforms.Android;
public class AudioService : IAudioService
{
    private MediaPlayer _player;

    public event EventHandler PlayFinished;
    public event EventHandler PlayFailed;
    public bool IsPlaying => _player?.IsPlaying ?? false;
    public double CurrentPosition => _player?.CurrentPosition / 1000 ?? 0;

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

    public Task PlayAsync(double position = 0)
    {
        _player.Start();
        _player.SeekTo((int)position * 1000);
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