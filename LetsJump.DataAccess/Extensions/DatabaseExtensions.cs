using Dapper;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Security;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsJump.DataAccess.Extensions
{
    public static class DatabaseExtensions
    {
        public static bool IsValidUser(this MySqlConnection connection, CommonData data)
        {
            if (!TokenManager.IsAdminToken(data.AccessToken))
            {
                int profileCount = connection.ExecuteScalar<int>("Select COUNT(*) from User_Login Where UserID=@UserId", new { UserID = data.UserID });
                return profileCount > 0 ? true : false;
            }
            return true;
        }
    }
}
