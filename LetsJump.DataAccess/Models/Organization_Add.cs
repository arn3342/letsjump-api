using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class Organization_Add
    {
        public string AccessToken { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        /// <summary>
        /// This property references the legal registration number of the company
        /// </summary>
        public string RegNo { get; set; }
        public string MediaPath { get; set; }
        public string Location { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string PhoneNo { get; set; }
        public string SupportEmail { get; set;}
        public int TotalReviews { get; set; }
        public decimal AvgReview { get; set; }
        public bool HasParking { get; set; }
        public bool HasWifi { get; set; }
        public bool HasRooms { get; set; }
        public bool HasSwimming { get; set; }
        public string SearchTags { get; set; }
        public bool IsRemoved { get; set; }
    }
}
