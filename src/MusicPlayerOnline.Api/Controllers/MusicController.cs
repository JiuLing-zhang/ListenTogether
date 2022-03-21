using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Controllers
{
    [Route("api/music")]
    [ApiController]
    [Authorize]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _musicService.GetOneAsync(id);
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> AddOrUpdate(Music music)
        {
            var response = await _musicService.AddOrUpdateAsync(music);
            return Ok(response);
        }

        [HttpPost("update-cache/{id}/{cachePath}")]
        public async Task<IActionResult> UpdateCache(string id, string cachePath)
        {
            var response = await _musicService.UpdateCacheAsync(id, cachePath);
            return Ok(response);
        }
    }
}
