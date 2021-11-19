using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class UserDetails : CommonData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public int Points { get; set; }
        public string AreaName { get; set; }
        public string Dob { get; set;}
    }
}
