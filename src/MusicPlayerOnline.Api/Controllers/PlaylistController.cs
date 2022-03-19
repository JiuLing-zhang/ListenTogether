using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Controllers
{
    [Route("api/playlist")]
    [ApiController]
    [Authorize]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _playlistService.GetAllAsync(user.Id);
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> AddOrUpdate(Playlist playlist)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _playlistService.AddOrUpdateAsync(user.Id, playlist);
            return Ok(response);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Remove(int id)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _playlistService.RemoveAsync(user.Id, id);
            return Ok(response);
        }

        [HttpPost("clear")]
        public async Task<IActionResult> RemoveAll()
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _playlistService.RemoveAllAsync(user.Id);
            return Ok(response);
        }
    }
}
