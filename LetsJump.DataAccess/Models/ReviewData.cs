using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class ReviewData
    {
        public DateTime CheckInDate { get; set; }
        public int ReviewID { get; set; }
        public string ReviewText { get; set; }
        public decimal ReviewRating { get; set;}
        public string ReviewMediaUrls { get; set; }
        public int OrganizationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
        public string ImageURL { get; set; }
        public bool IsEditable { get; set; }

        public void SetEditable(CommonData user)
        {
            IsEditable = UserAccess.VerifyToken(user);
        }
    }
}
