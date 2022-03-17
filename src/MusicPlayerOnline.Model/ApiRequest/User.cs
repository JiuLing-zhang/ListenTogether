using System.ComponentModel.DataAnnotations;

namespace MusicPlayerOnline.Model.Request
{
    public class User
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
