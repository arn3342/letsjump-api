using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class Report : CommonData
    {
        public int reportID { get; set; }
        public int reviewID { get; set; }
        public int reviewUserID { get; set; }
        public int reportUserID { get; set; }
        public string reportReason { get; set; }
        public bool isRemoved { get; set; }
    }
}
