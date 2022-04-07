using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Username is required for sign in")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required for sign in")]
        public string Password { get; set; }
        public string? Token { get; set; }
    }
}
