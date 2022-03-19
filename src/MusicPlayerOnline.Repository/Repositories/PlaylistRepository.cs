using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Repository.Entities;

namespace MusicPlayerOnline.Repository.Repositories
{
    public class PlaylistRepository
    {
        public async Task<Result> AddOrUpdateAsync(Playlist playlist)
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
                return new Result(1, "保存失败");
            }

            return new Result(0, "保存成功");
        }

        public async Task<List<PlaylistDto>> GetAllAsync(int userId)
        {
            var playlists = await DatabaseProvide.DatabaseAsync.Table<PlaylistEntity>().ToListAsync();
            return playlists.Select(x => new PlaylistDto()
            {
                MusicId = x.MusicId,
                MusicName = x.MusicName,
                MusicArtist = x.MusicArtist
            }).ToList();
        }

        public async Task<Result> RemoveAsync(int id)
        {
            await DatabaseProvide.DatabaseAsync.DeleteAsync<PlaylistEntity>(id);
            return new Result(0, "删除成功");
        }

        public async Task<Result> RemoveAllAsync()
        {
            await DatabaseProvide.DatabaseAsync.DeleteAllAsync<PlaylistEntity>();
            return new Result(0, "删除成功");
        }
    }
}
