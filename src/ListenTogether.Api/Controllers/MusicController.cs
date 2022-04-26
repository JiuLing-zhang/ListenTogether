using Microsoft.AspNetCore.Mvc;
using ListenTogether.Api.Authorization;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api.Request;

namespace ListenTogether.Api.Controllers
{
    [Route("api/music")]
    [ApiController]
    [Authorize]
    public class MusicController : ApiBaseController
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
        public async Task<IActionResult> AddOrUpdate(MusicRequest music)
        {
            var response = await _musicService.AddOrUpdateAsync(music);
            return Ok(response);
        }
    }
}
