using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.Authentication
{
    public class ResetPassword
    {
        [Required]
        public string Email { get; set; }
        public string Token { get; set; }
        [Required]

        public string NewPassword { get; set; }
        [Required]

        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
