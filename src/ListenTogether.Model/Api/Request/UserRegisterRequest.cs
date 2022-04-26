using System.ComponentModel.DataAnnotations;

namespace ListenTogether.Model.Api.Request;

public class UserRegisterRequest
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Nickname { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

}