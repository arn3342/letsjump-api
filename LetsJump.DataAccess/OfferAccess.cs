using Dapper;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Extensions;
using LetsJump.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace LetsJump.DataAccess
{
    public class OfferAccess : DataCredential
    {
        private string spPrefix = "sp_offer_";

        public bool Create(Offer offer)
        {
            var newOffer = 0;

            #region Checking if values are OK and proceeding
            connection.Open();

            if (connection.IsValidUser(offer))
            {
                //if(favorite.IsCurrentlyFavorite)
                //{
                //    newFavorite = connection.Execute("Delete from Favorites where UserID=" + favorite.UserID + " AND OrganizationID=" + favorite.OrganizationID);
                //}
                //else
                newOffer = connection.Execute(spPrefix + "create", offer, commandType: CommandType.StoredProcedure);
            }
            #endregion
            connection.Close();

            return IsSuccess(newOffer);
        }

        public (bool IsSuccess, IEnumerable<Offer> offers) Get(Offer offer)
        {
            //int records = connection.Execute("Select COUNT(*) From Favorites Where UserID=" + favorite.UserID + " AND OrganizationID=" + favorite.OrganizationID);
            IEnumerable<Offer> records = null;
            connection.Open();
            if (offer.UserID != 0)
            {
                var points = connection.QuerySingle<int>("Select Points From User_Details Where UserID=" + offer.UserID);
                if (IsSuccess(points))
                    records = connection.Query<Offer>("Select * From Offers where ConditionAbove < " + points + " AND ConditionBelow > " + points);
            }
            else
            {
                records = connection.Query<Offer>("Select * From Offers");
            }
            connection.Close();
            return (IsSuccess(records), records);
        }
    }
}
