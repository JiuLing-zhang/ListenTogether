using Microsoft.AspNetCore.Mvc;
using ListenTogether.Api.Entities;
using ListenTogether.Api.ErrorHandler;

namespace ListenTogether.Api.Controllers;
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