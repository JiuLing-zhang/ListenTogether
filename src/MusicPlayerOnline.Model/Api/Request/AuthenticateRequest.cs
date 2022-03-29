using System.ComponentModel.DataAnnotations;

namespace MusicPlayerOnline.Model.Api.Request;
public class AuthenticateRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}