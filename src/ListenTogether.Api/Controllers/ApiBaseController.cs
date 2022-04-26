using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Entities;
using MusicPlayerOnline.Api.ErrorHandler;

namespace MusicPlayerOnline.Api.Controllers;
public class ApiBaseController : ControllerBase
{
    protected int UserId => FindUserId();
    private int FindUserId()
    {

        var user = Request.HttpContext.Items["User"] as UserEntity;
        if (user == null)
        {
            throw new AppException("用户未登录");
        }

        return user.Id;
    }
}