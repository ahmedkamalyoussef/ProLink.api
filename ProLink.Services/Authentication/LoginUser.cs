using System.ComponentModel.DataAnnotations;

namespace ProLink.Application.Authentication
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
