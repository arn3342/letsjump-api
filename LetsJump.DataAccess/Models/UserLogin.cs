using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class UserLogin : CommonData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsSocialLogin { get; set; }
        public bool IsVerified { get; set; }
        public bool IsProfileComplete { get; set; }
    }
}
