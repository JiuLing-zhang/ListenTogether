using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Maui.Services;
internal class PlayerService : PlayerServiceAbstract
{
    //TODO 重命名
    private static IAudioService _audioService;

    public override double CurrentPosition => _audioService.CurrentPosition;

    /// <summary>
    /// 是否正在播放
    /// </summary>
    public override bool IsPlaying => _audioService.IsPlaying;

    /// <summary>
    /// 正在播放的歌曲信息
    /// </summary>
    public override Music CurrentMusic => _currentMusic;
    private Music _currentMusic;

    public override event EventHandler NewMusicAdded;
    public override event EventHandler IsPlayingChanged;
    public override event EventHandler PlayFinished;

    public PlayerService(IAudioService audioService)
    {
        _audioService = audioService;
        _audioService.PlayFinished += PlayFinished;
    }

    public override async Task PlayAsync(Music music, double position = 0)
    {
        var isChangeMusic = _currentMusic?.Id != music.Id;

        if (isChangeMusic)
        {
            _currentMusic = music;
            if (_audioService.IsPlaying)
            {
                await _audioService.PauseAsync();
            }
            await _audioService.InitializeAsync(_currentMusic.PlayUrl);
            await _audioService.PlayAsync(position);
            NewMusicAdded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await _audioService.PlayAsync(position);
        }
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }

    public override async Task PauseAsync()
    {
        await _audioService.PauseAsync();
        IsPlayingChanged?.Invoke(this, EventArgs.Empty);
    }
}