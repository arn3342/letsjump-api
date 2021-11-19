using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class PasswordReset
    {
        public string UniqueKey { get; set; }
        public string Password { get; set; }
        public string rePassword { get; set; }
    }
}
