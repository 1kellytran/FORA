using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fora.Shared
{
    public class UserStatusDTOModel
    {
        public bool IsLoggedIn { get; set; }
        public bool IsAdmin { get; set; }
    }
}
