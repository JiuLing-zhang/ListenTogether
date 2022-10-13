using ListenTogether.Data.Entities;
using ListenTogether.Data.Interfaces;
using ListenTogether.Model;

namespace ListenTogether.Data.Repositories.Local;
public class MusicCacheRepository : IMusicCacheRepository
{
    public async Task<MusicCache?> GetOneAsync(int id)
    {
        var cache = await DatabaseProvide.DatabaseAsync.Table<MusicCacheEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (cache == null)
        {
            return default;
        }

        return new MusicCache()
        {
            Id = cache.Id,
            MusicId = cache.MusicId,
            FileName = cache.FileName,
            Remark = cache.Remark
        };
    }

    public async Task<MusicCache?> GetOneByMuiscIdAsync(string musicId)
    {
        var cache = await DatabaseProvide.DatabaseAsync.Table<MusicCacheEntity>().FirstOrDefaultAsync(x => x.MusicId == musicId);
        if (cache == null)
        {
            return default;
        }

        return new MusicCache()
        {
            Id = cache.Id,
            MusicId = cache.MusicId,
            FileName = cache.FileName,
            Remark = cache.Remark
        };
    }

    public async Task<List<MusicCache>> GetAllAsync()
    {
        var caches = await DatabaseProvide.DatabaseAsync.Table<MusicCacheEntity>().OrderBy(x => x.Remark).ToListAsync();
        return caches.Select(x => new MusicCache()
        {
            Id = x.Id,
            MusicId = x.MusicId,
            FileName = x.FileName,
            Remark = x.Remark
        }).ToList();
    }

    public async Task<bool> AddOrUpdateAsync(string musicId, string fileName, string remark)
    {
        var dbCache = await DatabaseProvide.DatabaseAsync.Table<MusicCacheEntity>().FirstOrDefaultAsync(x => x.MusicId == musicId);
        int count;
        if (dbCache == null)
        {

            count = await DatabaseProvide.DatabaseAsync.InsertAsync(
                new MusicCacheEntity()
                {
                    MusicId = musicId,
                    FileName = fileName,
                    Remark = remark
                });
        }
        else
        {
            dbCache.FileName = fileName;
            count = await DatabaseProvide.DatabaseAsync.UpdateAsync(dbCache);
        }
        if (count == 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var count = await DatabaseProvide.DatabaseAsync.DeleteAsync<MusicCacheEntity>(id);
        if (count == 0)
        {
            return false;
        }
        return true;
    }
}