using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;
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

        public async Task<Result<MusicResponse>> GetOneAsync(string id)
        {
            var music = await _context.Musics.SingleOrDefaultAsync(x => x.Id == id);
            if (music == null)
            {
                return new Result<MusicResponse>(1, "歌曲不存在", null);
            }

            return new Result<MusicResponse>(0, "查询成功", new MusicResponse()
            {
                Id = id,
                Platform = music.Platform,
                PlatformInnerId = music.PlatformInnerId,
                PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
                Name = music.Name,
                Album = music.Album,
                Alias = music.Alias,
                Artist = music.Artist,
                Duration = music.Duration,
                ImageUrl = music.ImageUrl,
                Lyric = music.Lyric,
                PlayUrl = music.PlayUrl
            });
        }

        public async Task<Result> AddOrUpdateAsync(MusicRequest music)
        {
            var myMusic = await _context.Musics.SingleOrDefaultAsync(x => x.Id == music.Id);
            if (myMusic == null)
            {
                myMusic = new MusicEntity()
                {
                    Id = music.Id,
                    Platform = (int)music.Platform,
                    PlatformInnerId = music.PlatformInnerId,
                    Name = music.Name,
                    Album = music.Album,
                    Alias = music.Alias,
                    Artist = music.Artist,
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
                myMusic.PlatformInnerId = music.PlatformInnerId;
                myMusic.Name = music.Name;
                myMusic.Album = music.Album;
                myMusic.Alias = music.Alias;
                myMusic.Artist = music.Artist;
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
    }
}
