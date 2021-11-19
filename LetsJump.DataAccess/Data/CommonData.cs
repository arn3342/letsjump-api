using Dapper;
using LetsJump.DataAccess.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LetsJump.DataAccess.Data
{
    public class CommonData : DataCredential
    {
        public int UserID { get; set; }
        public string SearchTags { get; set; }

        private string _accessToken;
        public string AccessToken{ get { return _accessToken; } set { _accessToken = value; UserID = TokenManager.IsAdminToken(value) ? 0 : SetUserID(value, UserID); } }

        int SetUserID(string accessToken, int userID)
        {
            if (userID == 0)
                return TokenManager.DecipherID(accessToken);

            return userID;
        }
    }
}
