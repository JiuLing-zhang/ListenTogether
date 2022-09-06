using Microsoft.AspNetCore.Mvc;
using ListenTogether.Api.Authorization;
using ListenTogether.Api.Interfaces;
using ListenTogether.Model.Api.Request;

namespace ListenTogether.Api.Controllers
{
    [Route("api/my-favorite")]
    [ApiController]
    [Authorize]
    public class MyFavoriteController : ApiBaseController
    {
        private readonly IMyFavoriteService _myFavoriteService;
        public MyFavoriteController(IMyFavoriteService myFavoriteService)
        {
            _myFavoriteService = myFavoriteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _myFavoriteService.GetOneAsync(UserId, id);
            return Ok(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _myFavoriteService.GetAllAsync(UserId);
            return Ok(response);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetailAsync(int id)
        {
            var response = await _myFavoriteService.GetMyFavoriteDetailAsync(UserId, id);
            return Ok(response);
        }

        [HttpGet("name-exist/{myFavoriteName}")]
        public async Task<IActionResult> NameExistAsync(string myFavoriteName)
        {
            var response = await _myFavoriteService.NameExistAsync(UserId, myFavoriteName);
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> AddOrUpdateAsync(MyFavoriteRequest myFavorite)
        {
            var response = await _myFavoriteService.AddOrUpdateAsync(UserId, myFavorite);
            return Ok(response);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            var response = await _myFavoriteService.RemoveAsync(UserId, id);
            return Ok(response);
        }

        [HttpPost("add-music/{id}")]
        public async Task<IActionResult> AddMusicToMyFavoriteAsync(int id, MusicRequest music)
        {
            var response = await _myFavoriteService.AddMusicToMyFavoriteAsync(UserId, id, music);
            return Ok(response);
        }

        [HttpPost("remove-detail/{id}")]
        public async Task<IActionResult> RemoveDetailAsync(int id)
        {
            var response = await _myFavoriteService.RemoveDetailAsync(UserId, id);
            return Ok(response);
        }
    }
}
