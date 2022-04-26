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

    public async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword)
    {
        return await Search(platform, keyword, new List<MusicSearchResult>());
    }

    protected async Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword, List<MusicSearchResult> allResult)
    {
        if ((platform & _platform) == _platform)
        {
            await DoSearch(keyword, allResult);
        }
        if (_nextHandler != null)
        {
            await _nextHandler.Search(platform, keyword, allResult);
        }
        return allResult;
    }
    public abstract Task<List<MusicSearchResult>> DoSearch(string keyword, List<MusicSearchResult> allResult);
}
