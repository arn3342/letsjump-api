using Dapper;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Extensions;
using LetsJump.DataAccess.Models;
using System;
using System.Data;

namespace LetsJump.DataAccess
{
    public class CheckinAccess : DataCredential
    {
        private string spPrefix = "sp_checkin_";

        /// <summary>
        /// Function creates a new record in the database and updates if existing.
        /// </summary>
        /// <param name="checkin"></param>
        /// <returns></returns>
        public (bool IsSuccess, int CheckInID) Create(Checkin checkin)
        {
            connection.Open();
            var newCheckin = 0;
            var id = 0;
            #region Checking if values are OK
            if (connection.IsValidUser(checkin) && IsSuccess(checkin.UserID, checkin.OrganizationID))
            {
                //using (var multi = connection.QueryMultiple(@"exec " + spPrefix + "create @checkin; select LAST_INSERT_ID()", checkin))
                //{
                //    var foos = multi.Read<object>().ToList();
                //    var bars = multi.Read<object>().ToList();
                //}   
                var random = new Random();
                id = random.Next(1000, 9999);
                checkin.CheckinID = id;
                newCheckin = connection.Execute(spPrefix + "create", checkin, commandType: CommandType.StoredProcedure);

                //var lastCheckin = connection.Query("Select LAST_INSERT_ID()");
            }
            #endregion
            connection.Close();

            return (IsSuccess(newCheckin), id );
        }
    }
}