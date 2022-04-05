using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared
{
    public class UserDTOModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Username is to long!")]
        [MinLength(3, ErrorMessage = "Username too short!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Verify Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password dont match")]
        public string VerifiedPassword { get; set; }
       
    }
}
