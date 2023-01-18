using ListenTogether.Model;
using ListenTogether.Model.Enums;

namespace ListenTogether.Network.SearchMusic;
public abstract class SearchAbstract
{
    private SearchAbstract? _nextHandler;
    private readonly PlatformEnum _platform;
    protected SearchAbstract(PlatformEnum platform)
    {
        _platform = platform;
    }
    public void SetNextHandler(SearchAbstract nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public async Task<List<MusicResultShow>> SearchAsync(PlatformEnum platform, string keyword)
    {
        return await SearchAsync(platform, keyword, new List<MusicResultShow>());
    }

    protected async Task<List<MusicResultShow>> SearchAsync(PlatformEnum platform, string keyword, List<MusicResultShow> allResult)
    {
        if ((platform & _platform) == _platform)
        {
            await DoSearchAsync(keyword, allResult);
        }
        if (_nextHandler != null)
        {
            await _nextHandler.SearchAsync(platform, keyword, allResult);
        }
        return allResult;
    }
    public abstract Task<List<MusicResultShow>> DoSearchAsync(string keyword, List<MusicResultShow> allResult);
}
