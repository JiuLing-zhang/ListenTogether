using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.Api;
using MusicPlayerOnline.Model.Api.Request;
using MusicPlayerOnline.Model.Api.Response;

namespace MusicPlayerOnline.Api.Services
{
    public class MyFavoriteService : IMyFavoriteService
    {
        private readonly DataContext _context;
        public MyFavoriteService(DataContext dataContext)
        {
            _context = dataContext;
        }


        public async Task<Result<MyFavoriteResponse>> GetOneAsync(int userId, int id)
        {
            var myFavorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.Id == id && x.UserBaseId == userId);
            if (myFavorite == null)
            {
                return new Result<MyFavoriteResponse>(1, "我的收藏不存在", null);
            }

            return new Result<MyFavoriteResponse>(0, "查询成功", new MyFavoriteResponse()
            {
                Id = myFavorite.Id,
                Name = myFavorite.Name,
                ImageUrl = myFavorite.ImageUrl,
                MusicCount = myFavorite.MusicCount
            });
        }

        public async Task<List<MyFavoriteResponse>?> GetAllAsync(int userId)
        {
            var myFavorites = await _context.MyFavorites
                .Where(x => x.UserBaseId == userId)
                .Select(x => new MyFavoriteResponse()
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    MusicCount = x.MusicCount,
                    Name = x.Name
                })
                .ToListAsync();
            return myFavorites;
        }
        public async Task<Result<MyFavoriteResponse>> AddOrUpdateAsync(int userId, MyFavoriteRequest myFavorite)
        {
            var favorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.Name == myFavorite.Name && x.UserBaseId == userId);
            if (favorite == null)
            {
                favorite = new MyFavoriteEntity()
                {
                    Name = myFavorite.Name,
                    UserBaseId = userId,
                    ImageUrl = myFavorite.ImageUrl
                };
                _context.MyFavorites.Add(favorite);
            }
            else
            {
                favorite.Name = myFavorite.Name;
                favorite.UserBaseId = userId;
                favorite.ImageUrl = myFavorite.ImageUrl;
                _context.MyFavorites.Update(favorite);
            }

            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result<MyFavoriteResponse>(1, "保存失败", null);
            }

            return new Result<MyFavoriteResponse>(0, "保存成功", new MyFavoriteResponse()
            {
                Id = myFavorite.Id,
                Name = myFavorite.Name,
                ImageUrl = myFavorite.ImageUrl
            });
        }

        public async Task<Result> RemoveAsync(int userId, int id)
        {
            var myFavorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.Id == id && x.UserBaseId == userId);
            if (myFavorite == null)
            {
                return new Result(0, "成功");
            }

            _context.MyFavorites.Remove(myFavorite);

            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(1, "删除失败");
            }
            return new Result(0, "删除成功");
        }
        public async Task<Result> AddMusicToMyFavorite(int userId, int id, MusicRequest music)
        {
            var favorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (favorite == null)
            {
                return new Result(1, "添加失败：我的收藏不存在");
            }

            var favoriteDetail = favorite.Details.SingleOrDefault(x => x.MusicId == music.Id);
            if (favoriteDetail == null)
            {
                favoriteDetail = new MyFavoriteDetailEntity()
                {
                    Platform = (int)music.Platform,
                    MyFavoriteId = id,
                    MusicName = music.Name,
                    MusicId = music.Id,
                    MusicAlbum = music.Album,
                    MusicArtist = music.Artist
                };
                favorite.Details.Add(favoriteDetail);
            }
            else
            {
                favoriteDetail.MusicId = music.Id;
                favoriteDetail.MusicName = music.Name;
                favoriteDetail.MusicArtist = music.Artist;
                favoriteDetail.MusicAlbum = music.Album;
            }

            _context.MyFavorites.Update(favorite);
            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(3, "添加失败");
            }
            return new Result(0, "添加成功");

        }

        public async Task<List<MyFavoriteDetailResponse>?> GetMyFavoriteDetail(int userId, int id)
        {
            var myFavorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.UserBaseId == userId && x.Id == id);
            if (myFavorite == null)
            {
                return default;
            }

            var detail = myFavorite.Details.Select(x => new MyFavoriteDetailResponse()
            {
                Id = x.Id,
                MyFavoriteId = x.MyFavoriteId,
                Platform = x.Platform,
                MusicId = x.MusicId,
                MusicName = x.MusicName,
                MusicAlbum = x.MusicAlbum,
                MusicArtist = x.MusicArtist,
            }).ToList();
            return detail;
        }
    }
}
