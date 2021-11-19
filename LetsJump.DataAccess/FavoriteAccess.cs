using Dapper;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Extensions;
using LetsJump.DataAccess.Models;
using LetsJump.DataAccess.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LetsJump.DataAccess
{
    public class FavoriteAccess : DataCredential
    {
        private string spPrefix = "sp_favorite_";

        public bool Create(Favorite favorite)
        {
            var newFavorite = 0;

            #region Checking if values are OK and proceeding
            connection.Open();
            
            if (connection.IsValidUser(favorite) && IsSuccess(favorite.UserID, favorite.OrganizationID))
            {
                if(favorite.IsCurrentlyFavorite)
                {
                    newFavorite = connection.Execute("Delete from Favorites where UserID=" + favorite.UserID + " AND OrganizationID=" + favorite.OrganizationID);
                }
                else
                newFavorite = connection.Execute(spPrefix + "create", favorite, commandType: CommandType.StoredProcedure);
            }
            #endregion
            connection.Close();
            
            return IsSuccess(newFavorite);
        }

        public bool Get(Favorite favorite)
        {
            int records = connection.ExecuteScalar<int>("Select COUNT(*) From Favorites Where UserID=" + favorite.UserID + " AND OrganizationID=" + favorite.OrganizationID);
            return IsSuccess(records);
        }
    }
}
