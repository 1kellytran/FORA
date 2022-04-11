using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Fora.Shared
{
    public class PasswordDTOModel
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "New Password needs to be confrimed")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match")]
        public string NewPasswordConfirmed { get; set; }
    }
}
