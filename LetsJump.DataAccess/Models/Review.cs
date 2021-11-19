using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class Review : CommonData
    {
        public int ReviewID { get; set; }
        public int OrganizationID { get; set; }
        public string ReviewText { get; set; }
        public string ReviewMediaURLs { get; set; }
        public decimal ReviewRating { get; set; }
        public int ReviewLikeCount { get; set; }
        public int ReviewDislikeCount { get; set; }
        public int CheckInID { get; set; }
    }
}