using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using ListenTogether.Api.DbContext;
using ListenTogether.Api.Entities;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;
using ListenTogether.Model.Enums;

namespace ListenTogether.Api.Services
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
                Platform = (PlatformEnum)music.Platform,
                IdOnPlatform = music.IdOnPlatform,
                PlatformName = ((PlatformEnum)music.Platform).GetDescription(),
                Name = music.Name,
                Album = music.Album,
                Artist = music.Artist,
                ImageUrl = music.ImageUrl,
                ExtendDataJson = music.ExtendData,
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
                    IdOnPlatform = music.IdOnPlatform,
                    Name = music.Name,
                    Album = music.Album,
                    Artist = music.Artist,
                    ImageUrl = music.ImageUrl,
                    ExtendData = music.ExtendDataJson ?? "",
                    CreateTime = DateTime.Now
                };
                _context.Musics.Add(myMusic);
            }
            else
            {
                myMusic.Id = music.Id;
                myMusic.Platform = (int)music.Platform;
                myMusic.IdOnPlatform = music.IdOnPlatform;
                myMusic.Name = music.Name;
                myMusic.Album = music.Album;
                myMusic.Artist = music.Artist;
                myMusic.ImageUrl = music.ImageUrl;
                myMusic.ExtendData = music.ExtendDataJson ?? "";
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
