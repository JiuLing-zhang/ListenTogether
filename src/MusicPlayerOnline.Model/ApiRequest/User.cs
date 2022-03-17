using System.ComponentModel.DataAnnotations;

namespace MusicPlayerOnline.Model.ApiRequest
{
    public class User
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
