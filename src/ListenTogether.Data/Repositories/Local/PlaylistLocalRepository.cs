using ListenTogether.Data.Entities;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Data.Repositories.Local;
public class PlaylistLocalRepository : IPlaylistRepository
{
    public async Task<bool> AddOrUpdateAsync(Playlist playlist)
    {
        int count;
        var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().FirstOrDefaultAsync(x => x.MusicId == playlist.Id);
        if (playlists != null)
        {
            return true;
        }

        playlists = new PlaylistEntity()
        {
            Platform = playlist.Platform,
            MusicId = playlist.Id,
            IdOnPlatform = playlist.IdOnPlatform,
            Artist = playlist.Artist,
            Name = playlist.Name,
            Album = playlist.Album,
            ImageUrl = playlist.ImageUrl,
            DurationMillisecond = (int)playlist.Duration.TotalMilliseconds,
            ExtendDataJson = playlist.ExtendDataJson ?? "",
            EditTime = DateTime.Now
        };
        count = await DatabaseProvide.DatabaseAsync.InsertAsync(playlists);

        if (count == 0)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> AddOrUpdateAsync(List<Playlist> playlists)
    {
        await DatabaseProvide.DatabaseAsync.RunInTransactionAsync(tran =>
        {
            foreach (var playlist in playlists)
            {
                var playlistEntity = new PlaylistEntity()
                {
                    Platform = playlist.Platform,
                    MusicId = playlist.Id,
                    IdOnPlatform = playlist.IdOnPlatform,
                    Artist = playlist.Artist,
                    Name = playlist.Name,
                    Album = playlist.Album,
                    ImageUrl = playlist.ImageUrl,
                    ExtendDataJson = playlist.ExtendDataJson ?? "",
                    DurationMillisecond = (int)playlist.Duration.TotalMilliseconds,
                    EditTime = DateTime.Now
                };
                tran.InsertOrReplace(playlistEntity);
            }
            tran.Commit();
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
            Id = musicId,
            Name = playlistEntity.Name,
            Album = playlistEntity.Album,
            Artist = playlistEntity.Artist,
            IdOnPlatform = playlistEntity.IdOnPlatform,
            Platform = playlistEntity.Platform,
            ImageUrl = playlistEntity.ImageUrl,
            ExtendDataJson = playlistEntity.ExtendDataJson,
            Duration = TimeSpan.FromMilliseconds(playlistEntity.DurationMillisecond),
            EditTime = playlistEntity.EditTime
        };
    }
    public async Task<List<Playlist>> GetAllAsync()
    {
        var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().ToListAsync();
        return playlists.Select(x => new Playlist()
        {
            Platform = x.Platform,
            IdOnPlatform = x.IdOnPlatform,
            Id = x.MusicId,
            Name = x.Name,
            Artist = x.Artist,
            Album = x.Album,
            ImageUrl = x.ImageUrl,
            Duration = TimeSpan.FromMilliseconds(x.DurationMillisecond),
            ExtendDataJson = x.ExtendDataJson,
            EditTime = x.EditTime
        }).ToList();
    }

    public async Task<bool> RemoveAsync(string musicId)
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