using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.Authentication
{
   public class ConfirmRequest
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
