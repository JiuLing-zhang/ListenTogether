using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.Api.Request;

namespace MusicPlayerOnline.Api.Controllers
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
        public async Task<IActionResult> Get(int id)
        {
            var response = await _myFavoriteService.GetOneAsync(UserId, id);
            return Ok(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _myFavoriteService.GetAllAsync(UserId);
            return Ok(response);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var response = await _myFavoriteService.GetMyFavoriteDetail(UserId, id);
            return Ok(response);
        }

        [HttpGet("name-exist/{myFavoriteName}")]
        public async Task<IActionResult> NameExist(string myFavoriteName)
        {
            var response = await _myFavoriteService.NameExist(UserId, myFavoriteName);
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> AddOrUpdate(MyFavoriteRequest myFavorite)
        {
            var response = await _myFavoriteService.AddOrUpdateAsync(UserId, myFavorite);
            return Ok(response);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _myFavoriteService.RemoveAsync(UserId, id);
            return Ok(response);
        }

        [HttpPost("add-music/{id}")]
        public async Task<IActionResult> AddMusicToMyFavorite(int id, MusicRequest music)
        {
            var response = await _myFavoriteService.AddMusicToMyFavorite(UserId, id, music);
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
