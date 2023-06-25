using System.ComponentModel.DataAnnotations;

namespace ListenTogether.Model.Api.Request;
public class UserEditRequest
{
    [MinLength(4, ErrorMessage = "用户名长度至少4位")]
    [MaxLength(20, ErrorMessage = "用户名长度最多20位")]
    public string? Username { get; set; }

    [MinLength(4, ErrorMessage = "用户名长度至少4位")]
    [MaxLength(20, ErrorMessage = "用户名长度最多20位")]
    public string? Nickname { get; set; }

    public string? AvatarUrl { get; set; }
}