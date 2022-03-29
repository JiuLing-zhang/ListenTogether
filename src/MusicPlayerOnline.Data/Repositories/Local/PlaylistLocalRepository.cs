using MusicPlayerOnline.Data.Entities;
using MusicPlayerOnline.Data.Interfaces;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Data.Repositories.Local;
public class PlaylistLocalRepository : IPlaylistRepository
{
    public async Task<bool> AddOrUpdateAsync(Playlist playlist)
    {
        int count;
        var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().FirstOrDefaultAsync(x => x.MusicId == playlist.MusicId);
        if (playlists == null)
        {
            playlists = new PlaylistEntity()
            {
                MusicId = playlist.MusicId,
                MusicArtist = playlist.MusicArtist,
                MusicName = playlist.MusicName
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(playlists);
        }
        else
        {
            playlists.MusicArtist = playlist.MusicArtist;
            playlists.MusicName = playlist.MusicName;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(playlists);
        }
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<List<Playlist>?> GetAllAsync()
    {
        var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().ToListAsync();

        return playlists?.Select(x => new Playlist()
        {
            MusicId = x.MusicId,
            MusicName = x.MusicName,
            MusicArtist = x.MusicArtist
        }).ToList();
    }

    public async Task<bool> RemoveAsync(int id)
    {
        await DatabaseProvide.DatabaseAsync.DeleteAsync<PlaylistEntity>(id);
        return true;
    }

    public async Task<bool> RemoveAllAsync()
    {
        await DatabaseProvide.DatabaseAsync.DeleteAllAsync<PlaylistEntity>();
        return true;
    }
}