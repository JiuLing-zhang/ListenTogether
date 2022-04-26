using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Network.MusicProvider;
public interface IMusicProvider
{
    Task<(bool IsSucceed, string ErrMsg, List<MusicSearchResult>? musics)> Search(string keyword);
    Task<Music?> GetMusicDetail(MusicSearchResult sourceMusic);
    Task<Music?> UpdatePlayUrl(Music music);
}