using System.ComponentModel.DataAnnotations;

namespace ListenTogether.Model.Api.Request;

public class UserRegisterRequest
{
    [Required]
    [MinLength(4, ErrorMessage = "用户名长度至少4位")]
    [MaxLength(20, ErrorMessage = "用户名长度最多20位")]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(4, ErrorMessage = "昵称长度至少4位")]
    [MaxLength(20, ErrorMessage = "昵称长度最多20位")]
    public string Nickname { get; set; } = null!;

    [Required]
    [MinLength(4, ErrorMessage = "密码长度至少4位")]
    [MaxLength(20, ErrorMessage = "密码长度最多20位")]
    public string Password { get; set; } = null!;

}