using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.Request;

namespace MusicPlayerOnline.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login(User model)
        {
            var response = await _userService.Login(model, IpAddress());
            return Ok(response);
        }

        private string IpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
