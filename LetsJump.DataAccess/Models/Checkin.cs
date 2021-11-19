using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class Checkin : CommonData
    {
        public int CheckinID { get; set; }
        public int OrganizationID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
