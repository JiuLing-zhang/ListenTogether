using MusicPlayerOnline.Business.Interfaces;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Business.Services;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistRepository _repository;
    public PlaylistService(IPlaylistRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> AddToPlaylist(Music music)
    {
        var playlist = new Playlist()
        {
            MusicId = music.Id,
            MusicName = music.Name,
            MusicArtist = music.Artist
        };
        return await _repository.AddOrUpdateAsync(playlist);
    }

    public async Task<List<Playlist>?> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        return await _repository.RemoveAsync(id);
    }

    public async Task<bool> RemoveAllAsync()
    {
        return await _repository.RemoveAllAsync();
    }
}