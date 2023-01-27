﻿using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using ListenTogether.Api.DbContext;
using ListenTogether.Api.Entities;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api;
using ListenTogether.Model.Api.Request;
using ListenTogether.Model.Api.Response;
using ListenTogether.Model.Enums;

namespace ListenTogether.Api.Services;
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
            MusicCount = myFavorite.MusicCount,
            EditTime = myFavorite.EditTime
        });
    }

    public async Task<List<MyFavoriteResponse>> GetAllAsync(int userId)
    {
        var myFavorites = await _context.MyFavorites
            .Where(x => x.UserBaseId == userId)
            .Select(x => new MyFavoriteResponse()
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                MusicCount = x.MusicCount,
                Name = x.Name,
                EditTime = x.EditTime
            })
            .ToListAsync();
        return myFavorites;
    }

    public async Task<Result> NameExistAsync(int userId, string myFavoriteName)
    {
        var any = await _context.MyFavorites.AnyAsync(x => x.Name == myFavoriteName && x.UserBaseId == userId);
        if (any)
        {
            return new Result(1, "歌单名称已存在");
        }
        return new Result(0, "歌单名称不存在");
    }

    public async Task<Result<MyFavoriteResponse>> AddOrUpdateAsync(int userId, MyFavoriteRequest myFavorite)
    {
        DateTime editTime = DateTime.Now;
        var favorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.Id == myFavorite.Id && x.UserBaseId == userId);
        if (favorite == null)
        {
            favorite = new MyFavoriteEntity()
            {
                Name = myFavorite.Name,
                UserBaseId = userId,
                ImageUrl = myFavorite.ImageUrl ?? "",
                EditTime = editTime
            };
            _context.MyFavorites.Add(favorite);
        }
        else
        {
            favorite.Name = myFavorite.Name;
            favorite.UserBaseId = userId;
            favorite.ImageUrl = myFavorite.ImageUrl ?? "";
            favorite.EditTime = editTime;
            _context.MyFavorites.Update(favorite);
        }

        var count = await _context.SaveChangesAsync();
        if (count == 0)
        {
            return new Result<MyFavoriteResponse>(1, "保存失败", null);
        }

        return new Result<MyFavoriteResponse>(0, "保存成功", new MyFavoriteResponse()
        {
            Id = favorite.Id,
            Name = favorite.Name,
            ImageUrl = favorite.ImageUrl,
            EditTime = editTime
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
    public async Task<Result> AddMusicToMyFavoriteAsync(int userId, int id, string musicId)
    {
        var favorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.UserBaseId == userId && x.Id == id);
        if (favorite == null)
        {
            return new Result(1, "添加失败：我的收藏不存在");
        }

        var favoriteDetail = favorite.Details.SingleOrDefault(x => x.MyFavoriteId == id && x.MusicId == musicId);
        if (favoriteDetail == null)
        {
            favoriteDetail = new MyFavoriteDetailEntity()
            {
                MyFavoriteId = id,
                MusicId = musicId,
            };
            favorite.Details.Add(favoriteDetail);
        }

        //更新歌单图标，歌单可以前置添加，所以有可能会没有图标
        if (favorite.ImageUrl.IsEmpty())
        {
            var imageUrl = _context.Musics.First(x => x.Id == musicId).ImageUrl;
            favorite.ImageUrl = imageUrl;
        }
        favorite.EditTime = DateTime.Now;

        _context.MyFavorites.Update(favorite);
        var count = await _context.SaveChangesAsync();
        if (count == 0)
        {
            return new Result(2, "添加失败");
        }
        return new Result(0, "添加成功");

    }

    public async Task<List<MyFavoriteDetailResponse>> GetMyFavoriteDetailAsync(int userId, int id)
    {
        var result = new List<MyFavoriteDetailResponse>();
        var myFavorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.UserBaseId == userId && x.Id == id);
        if (myFavorite == null)
        {
            return result;
        }

        var data = from mfd in myFavorite.Details
                   join m in _context.Musics on mfd.MusicId equals m.Id
                   select new MyFavoriteDetailResponse
                   {
                       Id = mfd.Id,
                       MyFavoriteId = mfd.MyFavoriteId,
                       PlatformInt = m.Platform,
                       MusicId = m.Id,
                       MusicName = m.Name,
                       MusicAlbum = m.Album,
                       MusicArtist = m.Artist,
                   };

        result = data.ToList();
        return result;
    }

    public async Task<Result> RemoveDetailAsync(int userId, int id)
    {
        var myFavorite = await _context.MyFavorites.SingleOrDefaultAsync(x => x.Details.Any(o => o.Id == id) && x.UserBaseId == userId);
        if (myFavorite == null)
        {
            return new Result(1, "数据不存在");
        }

        var myFavoriteDetail = myFavorite.Details.First(x => x.Id == id);
        myFavorite.Details.Remove(myFavoriteDetail);
        myFavorite.EditTime = DateTime.Now;
        _context.MyFavorites.Update(myFavorite);

        var count = await _context.SaveChangesAsync();
        if (count == 0)
        {
            return new Result(2, "删除失败");
        }
        return new Result(0, "删除成功");
    }
}