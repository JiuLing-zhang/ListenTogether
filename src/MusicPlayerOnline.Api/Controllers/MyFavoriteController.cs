using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Controllers
{
    [Route("api/my-favorite")]
    [ApiController]
    [Authorize]
    public class MyFavoriteController : ControllerBase
    {
        private readonly IMyFavoriteService _myFavoriteService;
        public MyFavoriteController(IMyFavoriteService myFavoriteService)
        {
            _myFavoriteService = myFavoriteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _myFavoriteService.GetOneAsync(user.Id, id);
            return Ok(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _myFavoriteService.GetAllAsync(user.Id);
            return Ok(response);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _myFavoriteService.GetMyFavoriteDetail(user.Id, id);
            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> AddOrUpdate(MyFavorite myFavorite)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _myFavoriteService.AddOrUpdateAsync(user.Id, myFavorite);
            return Ok(response);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _myFavoriteService.RemoveAsync(user.Id, id);
            return Ok(response);
        }

        [HttpPost("add-music/{id}")]
        public async Task<IActionResult> AddMusicToMyFavorite(int id, MyFavoriteDetail myFavoriteDetail)
        {
            var user = Request.HttpContext.Items["User"] as UserEntity;
            var response = await _myFavoriteService.AddMusicToMyFavorite(user.Id, id, myFavoriteDetail);
            return Ok(response);
        }
    }
}
