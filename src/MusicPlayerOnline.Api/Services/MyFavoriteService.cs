using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Services
{
    public class MyFavoriteService : IMyFavoriteService
    {
        private readonly DataContext _context;
        public MyFavoriteService(DataContext dataContext)
        {
            _context = dataContext;
        }


        public async Task<Result<MyFavoriteDto>> GetOneAsync(int userId, int id)
        {
            var myFavorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.Id == id && x.UserBaseId == userId);
            if (myFavorite == null)
            {
                return new Result<MyFavoriteDto>(1, "我的收藏不存在", null);
            }

            return new Result<MyFavoriteDto>(0, "查询成功", new MyFavoriteDto()
            {
                Id = myFavorite.Id,
                Name = myFavorite.Name,
                ImageUrl = myFavorite.ImageUrl,
                MusicCount = myFavorite.MusicCount
            });
        }

        public async Task<List<MyFavoriteDto>?> GetAllAsync(int userId)
        {
            var myFavorites = await _context.MyFavorites
                .Where(x => x.UserBaseId == userId)
                .Select(x => new MyFavoriteDto()
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    MusicCount = x.MusicCount,
                    Name = x.Name
                })
                .ToListAsync();
            return myFavorites;
        }
        public async Task<Result> AddOrUpdateAsync(int userId, MyFavorite myFavorite)
        {
            var favorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.Id == myFavorite.Id && x.UserBaseId == userId);
            if (favorite == null)
            {
                favorite = new MyFavoriteEntity()
                {
                    Id = myFavorite.Id,
                    Name = myFavorite.Name,
                    UserBaseId = userId,
                    ImageUrl = myFavorite.ImageUrl
                };
                _context.MyFavorites.Add(favorite);
            }
            else
            {
                favorite.Id = myFavorite.Id;
                favorite.Name = myFavorite.Name;
                favorite.UserBaseId = userId;
                favorite.ImageUrl = myFavorite.ImageUrl;
                _context.MyFavorites.Update(favorite);
            }

            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(1, "保存失败");
            }

            return new Result(0, "保存成功");
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
        public async Task<Result> AddMusicToMyFavorite(int userId, int id, MyFavoriteDetail myFavoriteDetail)
        {
            var favorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.UserBaseId == userId);
            if (favorite == null)
            {
                return new Result(1, "添加失败：我的收藏不存在");
            }

            var favoriteDetail = favorite.Details.SingleOrDefault(x => x.MusicId == myFavoriteDetail.MusicId);
            if (favoriteDetail == null)
            {
                favoriteDetail = new MyFavoriteDetailEntity()
                {
                    Platform = (int)myFavoriteDetail.Platform,
                    MyFavoriteId = id,
                    MusicName = myFavoriteDetail.MusicName,
                    MusicId = myFavoriteDetail.MusicId,
                    MusicAlbum = myFavoriteDetail.MusicAlbum,
                    MusicArtist = myFavoriteDetail.MusicArtist
                };
                favorite.Details.Add(favoriteDetail);
            }
            else
            {
                favoriteDetail.MusicId = myFavoriteDetail.MusicId;
                favoriteDetail.MusicName = myFavoriteDetail.MusicName;
                favoriteDetail.MusicArtist = myFavoriteDetail.MusicArtist;
                favoriteDetail.MusicAlbum = myFavoriteDetail.MusicAlbum;
            }

            _context.MyFavorites.Update(favorite);
            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(3, "添加失败");
            }
            return new Result(0, "添加成功");

        }

        public async Task<List<MyFavoriteDetailDto>?> GetMyFavoriteDetail(int userId, int id)
        {
            var myFavorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.UserBaseId == userId && x.Id == id);
            if (myFavorite == null)
            {
                return default;
            }

            var detail = myFavorite.Details.Select(x => new MyFavoriteDetailDto()
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
