using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;
using MusicPlayerOnline.Model.Enums;

namespace MusicPlayerOnline.Api.Services
{
    public class MusicService : IMusicService
    {
        private readonly DataContext _context;
        public MusicService(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Result<MusicDto>> GetOneAsync(string id)
        {
            var music = await _context.Musics.SingleOrDefaultAsync(x => x.Id == id);
            if (music == null)
            {
                return new Result<MusicDto>(1, "歌曲不存在", null);
            }

            return new Result<MusicDto>(0, "查询成功", new MusicDto()
            {
                Id = id,
                Platform = music.Platform,
                PlatformId = music.PlatformId,
                PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
                Name = music.Name,
                Album = music.Album,
                Alias = music.Alias,
                Artist = music.Artist,
                CachePath = music.CachePath,
                Duration = music.Duration,
                ImageUrl = music.ImageUrl,
                Lyric = music.Lyric,
                PlayUrl = music.PlayUrl
            });
        }

        public async Task<Result> AddOrUpdateAsync(Music music)
        {
            var myMusic = await _context.Musics.SingleOrDefaultAsync(x => x.Id == music.Id);
            if (myMusic == null)
            {
                myMusic = new MusicEntity()
                {
                    Id = music.Id,
                    Platform = (int)music.Platform,
                    PlatformId = music.PlatformId,
                    Name = music.Name,
                    Album = music.Album,
                    Alias = music.Alias,
                    Artist = music.Artist,
                    CachePath = music.CachePath,
                    Duration = music.Duration,
                    ImageUrl = music.ImageUrl,
                    Lyric = music.Lyric,
                    PlayUrl = music.PlayUrl
                };
                _context.Musics.Add(myMusic);
            }
            else
            {
                myMusic.Id = music.Id;
                myMusic.Platform = (int)music.Platform;
                myMusic.PlatformId = music.PlatformId;
                myMusic.Name = music.Name;
                myMusic.Album = music.Album;
                myMusic.Alias = music.Alias;
                myMusic.Artist = music.Artist;
                myMusic.CachePath = music.CachePath;
                myMusic.Duration = music.Duration;
                myMusic.ImageUrl = music.ImageUrl;
                myMusic.Lyric = music.Lyric;
                myMusic.PlayUrl = music.PlayUrl;
                _context.Musics.Update(myMusic);
            }

            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(1, "保存失败");
            }

            return new Result(0, "保存成功");
        }

        public async Task<Result> UpdateCacheAsync(string id, string cachePath)
        {
            var music = await _context.Musics.SingleOrDefaultAsync(x => x.Id == id);
            if (music == null)
            {
                return new Result(1, "歌曲不存在");
            }

            //缓存路径没有改变时不更新
            if (music.CachePath == cachePath)
            {
                return new Result(0, "更新成功");
            }

            music.CachePath = cachePath;
            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(2, "更新失败");
            }

            return new Result(0, "更新成功");
        }
    }
}
