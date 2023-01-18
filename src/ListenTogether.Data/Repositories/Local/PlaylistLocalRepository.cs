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
                Platform = playlist.Platform,
                MusicId = playlist.MusicId,
                MusicIdOnPlatform = playlist.MusicIdOnPlatform,
                MusicArtist = playlist.MusicArtist,
                MusicName = playlist.MusicName,
                MusicAlbum = playlist.MusicAlbum,
                MusicImageUrl = playlist.MusicImageUrl,
                EditTime = DateTime.Now
            };
            count = await DatabaseProvide.DatabaseAsync.InsertAsync(playlists);
        }
        else
        {
            playlists.MusicArtist = playlist.MusicArtist;
            playlists.MusicName = playlist.MusicName;
            playlists.MusicAlbum = playlist.MusicAlbum;
            playlists.MusicImageUrl = playlist.MusicImageUrl;
            playlists.EditTime = DateTime.Now;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(playlists);
        }
        if (count == 0)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> AddOrUpdateAsync(List<Playlist> playlists)
    {
        await DatabaseProvide.DatabaseAsync.RunInTransactionAsync(async tran =>
        {
            foreach (var playlist in playlists)
            {
                var playlistEntity = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().FirstOrDefaultAsync(x => x.MusicId == playlist.MusicId);
                if (playlistEntity == null)
                {
                    playlistEntity = new PlaylistEntity()
                    {
                        Platform = playlist.Platform,
                        MusicId = playlist.MusicId,
                        MusicIdOnPlatform = playlist.MusicIdOnPlatform,
                        MusicArtist = playlist.MusicArtist,
                        MusicName = playlist.MusicName,
                        MusicAlbum = playlist.MusicAlbum,
                        MusicImageUrl = playlist.MusicImageUrl,
                        EditTime = DateTime.Now
                    };
                    tran.Insert(playlistEntity);
                }
                else
                {
                    playlistEntity.MusicArtist = playlist.MusicArtist;
                    playlistEntity.MusicName = playlist.MusicName;
                    playlistEntity.MusicAlbum = playlist.MusicAlbum;
                    playlistEntity.MusicImageUrl = playlist.MusicImageUrl;
                    playlistEntity.EditTime = DateTime.Now;
                    tran.Update(playlistEntity);
                }
            }
        });
        return true;
    }
    public async Task<Playlist?> GetOneAsync(string musicId)
    {
        var playlistEntity = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().FirstOrDefaultAsync(x => x.MusicId == musicId);
        if (playlistEntity == null)
        {
            return default;
        }
        return new Playlist()
        {
            MusicId = musicId,
            MusicName = playlistEntity.MusicName,
            MusicAlbum = playlistEntity.MusicAlbum,
            MusicArtist = playlistEntity.MusicArtist,
            MusicIdOnPlatform = playlistEntity.MusicIdOnPlatform,
            Platform = playlistEntity.Platform,
            MusicImageUrl = playlistEntity.MusicImageUrl,
            EditTime = playlistEntity.EditTime
        };
    }
    public async Task<List<Playlist>> GetAllAsync()
    {
        var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().ToListAsync();
        return playlists.Select(x => new Playlist()
        {
            Platform = x.Platform,
            MusicIdOnPlatform = x.MusicIdOnPlatform,
            MusicId = x.MusicId,
            MusicName = x.MusicName,
            MusicArtist = x.MusicArtist,
            MusicAlbum = x.MusicAlbum,
            MusicImageUrl = x.MusicImageUrl,
            EditTime = x.EditTime
        }).ToList();
    }

    public async Task<bool> RemoveAsync(int musicId)
    {
        await DatabaseProvide.DatabaseAsync.DeleteAsync<PlaylistEntity>(musicId);
        return true;
    }

    public async Task<bool> RemoveAllAsync()
    {
        await DatabaseProvide.DatabaseAsync.DeleteAllAsync<PlaylistEntity>();
        return true;
    }
}