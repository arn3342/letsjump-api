using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class Offer : CommonData
    {
        public int OfferID { get; set; }
        public string Title { get; set; }
        public string MediaURL { get; set; }
        public string Description { get; set; }
        public int ConditionAbove { get; set; }
        public int ConditionBelow { get; set; }
        public DateTime CreatedOn { get; set; }
        public string OrganizationIDs { get; set; }
    }
}
