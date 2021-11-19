using Dapper;
using LetsJump.DataAccess.Data;
using LetsJump.DataAccess.Models;
using System.Collections.Generic;
using System.Reflection;
using static System.String;
using static LetsJump.DataAccess.Extensions.StringExtensions;
using LetsJump.DataAccess.Security;
using System;
using System.Linq;
using System.Data;

namespace LetsJump.DataAccess
{
    public class OrganizationAccess : DataCredential
    {
        private string spPrefix = "sp_organization_";
        public bool Create(Organization_Add organization)
        {
            connection.Open();
            var newOrg = connection.Execute(spPrefix + "create", organization, commandType: CommandType.StoredProcedure);
            if(organization.OrganizationID != 0)
            {
                connection.Execute("Delete from Organization_Details_Requests where OrganizationId=" + organization.OrganizationID);
            }
            connection.Close();

            return IsSuccess(newOrg);
        }

        public bool RemoveRestore(Organization organization)
        {
            connection.Open();
            var newOrg = 0;
            if (organization.OrganizationID != 0)
            {
                newOrg = connection.Execute("Update Organization_Details SET IsRemoved =" + organization.IsRemoved +" where OrganizationId=" + organization.OrganizationID);
            }
            connection.Close();

            return IsSuccess(newOrg);
        }
        public bool RequestCreate(Organization_Add organization)
        {
            connection.Open();

            var newOrg = connection.Execute(spPrefix + "create_request", organization, commandType: CommandType.StoredProcedure);
            connection.Close();

            return IsSuccess(newOrg);
        }
        public (bool IsSuccess, IEnumerable<Organization> organization) Get(Organization organization)
        {
            #region Constructing the query
            string dbQuery = "Select * from Organization_Details ";

            if (TokenManager.IsAdminToken(organization.AccessToken))
            {
                dbQuery = dbQuery.Replace("where", Empty);
            }
            else /*if(organization.AccessToken == null)*/
            {
                //Selecting the propertynames of the properties with values only
                foreach (PropertyInfo property in organization.GetType().GetProperties())
                {
                    object value = property.GetValue(organization, null);
                    if (IsSuccess(value))
                    {
                        string fieldName = property.Name;
                        string fieldWithValue = Empty;

                        //Checking if property is "Tags" and setting "Like" or regular comparison
                        //if (property.Name.ToLower().Contains("tag") || property.Name.ToLower().Contains("location"))
                        //{

                        //}
                        if (property.Name.ToLower().Contains("avgreview"))
                        {
                            fieldWithValue = fieldName + ">=" + value;
                            fieldWithValue += " AND ";
                            dbQuery += fieldWithValue;
                        }
                        else if (!property.Name.ToLower().Contains("has"))
                        {
                            dbQuery += dbQuery.Contains("where") ? "" : " where ";
                            fieldWithValue = "OrganizationName LIKE '%" + value + "%' OR SearchTags Like '%" + value + "%' OR Location Like '%" + value + "%'";
                            dbQuery += fieldWithValue;
                        }
                    }
                }

                dbQuery = dbQuery.ReplaceLastOccurrence(" AND ", Empty);

            }
            #endregion

            connection.Open();
            IEnumerable<Organization> _org = null;
            try
            {
                _org = connection.Query<Organization>(dbQuery);
                _org.All(x => { x.AccessToken = organization.AccessToken; return true; });
            }
            catch (Exception ex) { var error = ex.Message; }
            connection.Close();

            return (IsSuccess(_org), _org);
        }

        public (bool IsSuccess, IEnumerable<Organization> organization) GetRequests(Organization organization)
        {
            #region Constructing the query
            string dbQuery = "Select * from Organization_Details_Requests";

            if (TokenManager.IsAdminToken(organization.AccessToken))
            {
                dbQuery = dbQuery.Replace("where", Empty);
            }
            else /*if(organization.AccessToken == null)*/
            {
                //Selecting the propertynames of the properties with values only
                foreach (PropertyInfo property in organization.GetType().GetProperties())
                {
                    object value = property.GetValue(organization, null);
                    if (IsSuccess(value))
                    {
                        string fieldName = property.Name;
                        string fieldWithValue = Empty;

                        //Checking if property is "Tags" and setting "Like" or regular comparison
                        //if (property.Name.ToLower().Contains("tag") || property.Name.ToLower().Contains("location"))
                        //{

                        //}
                        if (property.Name.ToLower().Contains("avgreview"))
                        {
                            fieldWithValue = fieldName + ">=" + value;
                            fieldWithValue += " AND ";
                            dbQuery += fieldWithValue;
                        }
                        else if (!property.Name.ToLower().Contains("has"))
                        {
                            dbQuery += dbQuery.Contains("where") ? "" : " where ";
                            fieldWithValue = "OrganizationName LIKE '%" + value + "%' OR SearchTags Like '%" + value + "%' OR Location Like '%" + value + "%'";
                            dbQuery += fieldWithValue;
                        }
                    }
                }

                dbQuery = dbQuery.ReplaceLastOccurrence(" AND ", Empty);

            }
            #endregion

            connection.Open();
            IEnumerable<Organization> _org = null;
            try
            {
                _org = connection.Query<Organization>(dbQuery);
            }
            catch (Exception ex) { var error = ex.Message; }
            connection.Close();

            return (IsSuccess(_org), _org);
        }
    }
}
