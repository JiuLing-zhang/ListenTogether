﻿using Microsoft.EntityFrameworkCore;
using MusicPlayerOnline.Api.DbContext;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model;
using MusicPlayerOnline.Model.ApiRequest;
using MusicPlayerOnline.Model.ApiResponse;

namespace MusicPlayerOnline.Api.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly DataContext _context;
        public PlaylistService(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Result> AddOrUpdateAsync(int userId, Playlist playlist)
        {
            var playlists = await _context.Playlists.SingleOrDefaultAsync(x => x.UserBaseId == userId && x.MusicId == playlist.MusicId);
            if (playlists == null)
            {
                playlists = new PlaylistEntity()
                {
                    UserBaseId = userId,
                    MusicId = playlist.MusicId,
                    MusicArtist = playlist.MusicArtist,
                    MusicName = playlist.MusicName
                };
                _context.Playlists.Add(playlists);
            }
            else
            {
                playlists.MusicArtist = playlist.MusicArtist;
                playlists.MusicName = playlist.MusicName;
                _context.Playlists.Update(playlists);
            }

            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(1, "保存失败");
            }

            return new Result(0, "保存成功");
        }

        public async Task<List<PlaylistDto>> GetAllAsync(int userId)
        {
            var playlists = await _context.Playlists
                .Where(x => x.UserBaseId == userId)
                .Select(x => new PlaylistDto()
                {
                    MusicId = x.MusicId,
                    MusicName = x.MusicName,
                    MusicArtist = x.MusicArtist
                })
                .ToListAsync();
            return playlists;
        }

        public async Task<Result> RemoveAsync(int userId, string musicId)
        {
            var playlist = await _context.Playlists.SingleOrDefaultAsync(x => x.MusicId == musicId && x.UserBaseId == userId);
            if (playlist == null)
            {
                return new Result(0, "删除成功");
            }

            _context.Playlists.Remove(playlist);

            var count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                return new Result(1, "删除失败");
            }
            return new Result(0, "删除成功");
        }

        public async Task<Result> RemoveAllAsync(int userId)
        {
            var playlists = _context.Playlists.Where(x => x.UserBaseId == userId);
            _context.Playlists.RemoveRange(playlists);
            var count = await _context.SaveChangesAsync();
            if (count >= 0)
            {
                return new Result(0, "删除成功");
            }
            return new Result(1, "删除失败");
        }
    }
}
