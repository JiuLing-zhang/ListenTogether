using MusicPlayerOnline.Model.Enums;
using MusicPlayerOnline.Model.Network;

namespace MusicPlayerOnline.Network.BuildMusicDetail;
public abstract class BuildAbstract
{
    private BuildAbstract? _nextHandler;
    private readonly PlatformEnum _platform;
    protected BuildAbstract(PlatformEnum platform)
    {
        _platform = platform;
    }
    public void SetNextHandler(BuildAbstract nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public async Task<Music?> Build(MusicSearchResult music)
    {
        if (music.Platform == _platform)
        {
            return await DoBuild(music);
        }

        if (_nextHandler != null)
        {
            return await _nextHandler.Build(music);
        }

        throw new Exception("未找到对应的构建工具");
    }
    public abstract Task<Music?> DoBuild(MusicSearchResult music);
}