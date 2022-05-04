﻿using Microsoft.AspNetCore.Mvc;
using ListenTogether.Api.Authorization;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api.Request;

namespace ListenTogether.Api.Controllers
{
    [Route("api/playlist")]
    [ApiController]
    [Authorize]
    public class PlaylistController : ApiBaseController
    {
        private readonly IPlaylistService _playlistService;
        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _playlistService.GetAllAsync(UserId);
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> AddOrUpdate(PlaylistRequest playlist)
        {
            var response = await _playlistService.AddOrUpdateAsync(UserId, playlist);
            return Ok(response);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _playlistService.RemoveAsync(UserId, id);
            return Ok(response);
        }

        [HttpPost("clear")]
        public async Task<IActionResult> RemoveAll()
        {
            var response = await _playlistService.RemoveAllAsync(UserId);
            return Ok(response);
        }
    }
}