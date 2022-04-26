using System.ComponentModel.DataAnnotations;

namespace ListenTogether.Model.Api.Request;
public class AuthenticateRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}