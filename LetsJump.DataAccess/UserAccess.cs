using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Dapper;
using System.Data;
using LetsJump.DataAccess.Models;
using LetsJump.DataAccess.Security;
using MySqlConnector;
using LetsJump.DataAccess.Tools;
using System.Web;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Extensions;
using System.Reflection;

namespace LetsJump.DataAccess
{
    public class UserAccess : DataCredential
    {
        private string spPrefix = "sp_user_";

        public (bool IsSuccess, object User) Create(UserLogin user)
        {
            #region Hashing password
            user.Password = Hashing.HashPassword(user.Password);
            #endregion

            connection.Open();
            var newUser = connection.Execute(spPrefix + "create", user, commandType: CommandType.StoredProcedure);
            var getUser = connection.QuerySingle("Select * from User_Login Where Email=@Email", new { Email = user.Email });
            connection.Close();

            int UID = getUser != null ? getUser.UserID : 0;
            return (IsSuccess(newUser), new { UserID = UID });
        }


        public (bool IsSuccess, UserDetails newProfile) UpdateProfile(UserDetails user)
        {
            int affectedRows = 0;
            UserDetails updatedProfile = null;
            connection.Open();
            if (connection.IsValidUser(user))
            {
                affectedRows = connection.Execute(spPrefix + "updateProfile", user, commandType: CommandType.StoredProcedure);
                connection.Execute("Update User_Login SET IsProfileComplete = 1 Where UserId=@UserId", new { UserID = user.UserID });
                updatedProfile = connection.QuerySingle<UserDetails>("Select * from User_Details Where UserID=@UserId", new { UserID = user.UserID });
                updatedProfile.AccessToken = TokenManager.GenerateAccessToken(updatedProfile.UserID);
            }
            connection.Close();

            return (IsSuccess(affectedRows), updatedProfile);
        }

        public (bool IsSuccess, object Profile) GetProfile(UserDetails user)
        {
            //int affectedRows = 0;
            UserDetails profile = null;
            IEnumerable<object> profileList = new List<object>();
            connection.Open();
            if (connection.IsValidUser(user))
            {
                string query = "Select * from User_Details Where UserID=" + user.UserID.ToString();
                string query2 = "SELECT * FROM user_details INNER JOIN user_login ON user_details.UserID=user_login.UserID";
                if (TokenManager.IsAdminToken(user.AccessToken))
                {
                    query = query2;
                    foreach (PropertyInfo property in user.GetType().GetProperties())
                    {
                        object value = property.GetValue(user, null);
                        if (IsSuccess(value) && !property.Name.Contains("Token"))
                        {
                            string fieldName = property.Name;
                            string fieldWithValue = String.Empty;
                            query += query.Contains("where") ? "" : " where ";
                            fieldWithValue = "FirstName LIKE '%" + value + "%' OR LastName Like '%" + value + "%' OR Email Like '%" + value + "%'";
                            query += fieldWithValue;
                        }
                    }
                }
                if (!TokenManager.IsAdminToken(user.AccessToken))
                {
                    profile = connection.QuerySingle<UserDetails>(query);
                    profile.AccessToken = user.AccessToken;

                    return (IsSuccess(profile), profile);
                }
                else
                {
                    profileList = connection.Query<object>(query);
                    return (IsSuccess(profileList), profileList);
                }
            }
            connection.Close();
            return (false, null);
        }

        public static bool VerifyToken(CommonData user)
        {
            var userData = new CommonData() { AccessToken = user.AccessToken };
            return userData.UserID == user.UserID;
        }
        public (bool IsSuccess, object User) LoginUser(UserLogin user)
        {
            connection.Open();
            var currentUser = connection.QuerySingleOrDefault<UserLogin>("Select * from User_Login Where Email=@Email", new { Email = user.Email });

            if (currentUser !=null && !currentUser.IsSocialLogin)
                currentUser = Hashing.ValidatePassword(user.Password, currentUser.Password) ? currentUser : null;

            connection.Close();

            int UID = currentUser != null ? currentUser.UserID : 0;
            return (IsSuccess(currentUser), new { AccessToken = TokenManager.GenerateAccessToken(UID), IsProfileComplete = currentUser.IsProfileComplete });
        }
        public (bool IsSuccess, object User) GetUserByID(UserLogin user)
        {
            connection.Open();
            var currentUser = connection.QuerySingleOrDefault<UserLogin>("Select * from User_Login Where UserID=@UserID", new { UserID = user.UserID });
            connection.Close();

            return (IsSuccess(currentUser), currentUser);
        }

        public (bool IsSuccess, object User) GetAllUsers(CommonData user)
        {
            connection.Open();
            IEnumerable<object> users;
            string query = "Select * from User_Login INNER JOIN User_Details ON User_login.UserID=User_Details.UserID";
            if(user.UserID != 0)
            {
                query += " AND User_login.UserID=" + user.UserID.ToString();
            }
            users = connection.Query<object>(query);
            connection.Close();

            return (IsSuccess(users), users);
        }

        public bool VerifyUser(string email, bool checkVerify = false)
        {
            connection.Open();
            var currentVerification = connection.QueryFirstOrDefault<UserLogin>("Select * from User_Login Where Email=_email", new { _email = email });
            var affectedRows = 0;
            if (!checkVerify)
            {
                affectedRows = connection.Execute(spPrefix + "updateVerification", new { _email = email }, commandType: CommandType.StoredProcedure);
            }

            return checkVerify ? currentVerification.IsVerified : IsSuccess(affectedRows);
        }

        public bool RequestResetPassword(string email)
        {
            var currentVerification = connection.QueryFirstOrDefault<UserLogin>("Select * from User_Login Where Email=email", new { email = email });
            if (currentVerification != null)
            {
                string uniqueCode = Hashing.GeneratePassword(3) + currentVerification.UserID.ToString();
                Messaging messaging = new Messaging() { messageType = Messaging.MessageType.Password_RequestReset, EmailOrPhone = email, UniqueString = uniqueCode };
                messaging.SendMessage(currentVerification);

                return true;
            }
            return false;
        }

        public bool ResetPassword(PasswordReset reset)
        {
            var affectedRows = 0;
            if (reset.UniqueKey.Length > 3)
            {
                int userId = Convert.ToInt32(reset.UniqueKey.Substring(3, reset.UniqueKey.Length - 3));
                var pwd = Hashing.HashPassword(reset.Password);
                affectedRows = connection.Execute(spPrefix + "resetPassword", new { uid = userId, password = pwd}, commandType: CommandType.StoredProcedure);
            }
            return IsSuccess(affectedRows);
        }
    }
}
