using ListenTogether.Data.Entities;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Data.Repositories.Local;
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
                PlatformName = playlist.PlatformName,
                MusicId = playlist.MusicId,
                MusicArtist = playlist.MusicArtist,
                MusicName = playlist.MusicName,
                MusicAlbum = playlist.MusicAlbum,
                EditTime = DateTime.Now
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(playlists);
        }
        else
        {
            playlists.MusicArtist = playlist.MusicArtist;
            playlists.MusicName = playlist.MusicName;
            playlists.EditTime = DateTime.Now;
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
            Id = x.Id,
            PlatformName = x.PlatformName,
            MusicId = x.MusicId,
            MusicName = x.MusicName,
            MusicArtist = x.MusicArtist,
            MusicAlbum = x.MusicAlbum,
            EditTime = x.EditTime
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