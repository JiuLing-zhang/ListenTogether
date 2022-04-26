using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenTogether.Model
{
    public class User
    {
        public string Username { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
