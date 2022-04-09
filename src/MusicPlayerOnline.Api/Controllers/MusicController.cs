using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.Api.Request;

namespace MusicPlayerOnline.Api.Controllers
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
