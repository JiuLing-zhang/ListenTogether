using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Authorization;
using MusicPlayerOnline.Api.Interfaces;
using MusicPlayerOnline.Model.ApiRequest;

namespace MusicPlayerOnline.Api.Controllers;
[ApiController]
[Route("api/user")]
public class UserController : ApiBaseController
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("reg")]
    public async Task<IActionResult> Register(User model)
    {
        var response = await _userService.Register(model);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(User model)
    {
        var response = await _userService.Login(model);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(AuthenticateInfo model)
    {
        var response = await _userService.RefreshToken(model.RefreshToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _userService.Logout(UserId);
        return Ok("ok");
    }
}