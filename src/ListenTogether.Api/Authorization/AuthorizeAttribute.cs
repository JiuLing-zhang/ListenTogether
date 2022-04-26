using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MusicPlayerOnline.Api.Entities;

namespace MusicPlayerOnline.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }

        var user = context.HttpContext.Items["User"] as UserEntity;
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "用户未登录" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}