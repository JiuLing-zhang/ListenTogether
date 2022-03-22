using System.ComponentModel.DataAnnotations;

namespace MusicPlayerOnline.Model.ApiRequest;
public class AuthenticateInfo
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}