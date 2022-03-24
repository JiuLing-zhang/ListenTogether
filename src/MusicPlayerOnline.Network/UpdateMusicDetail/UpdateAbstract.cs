using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;

namespace MusicPlayerOnline.Network.UpdateMusicDetail;
public abstract class UpdateAbstract
{
    private UpdateAbstract? _nextHandler;
    private readonly PlatformEnum _platform;
    protected UpdateAbstract(PlatformEnum platform)
    {
        _platform = platform;
    }
    public void SetNextHandler(UpdateAbstract nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public async Task<Music?> Update(Music music)
    {
        if (music.Platform == _platform)
        {
            return await DoUpdate(music);
        }

        if (_nextHandler != null)
        {
            return await _nextHandler.Update(music);
        }

        throw new Exception("未找到对应的构建工具");
    }
    public abstract Task<Music?> DoUpdate(Music music);
}