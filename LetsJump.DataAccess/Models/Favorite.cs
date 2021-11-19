using LetsJump.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Models
{
    public class Favorite : CommonData
    {
        public int FavoriteID { get; set; }
        public int OrganizationID { get; set; }
        public int ItemID { get; set; }
        public bool IsCurrentlyFavorite { get; set; }
    }
}
