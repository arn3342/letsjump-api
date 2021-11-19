using Dapper;
using LetsJump.DataAccess.Extensions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetsJump.DataAccess.Data
{
    public class DataCredential
    {
        public const string CompanyName = "LetsJump";
        public const string CompanyMotto = "We look forward to giving you an amazing experience. Enjoy!";

        public const string connectionUri = "Server=localhost;Port=3306;Database=letsjump_db;Uid=root;Pwd=;AllowUserVariables=True;";
        public const string SMS_to_API_Key = "Y7FHD5bvmJhMXNesohQhY4ukbBXns0Z1";

        public const string SMTP_Host = "";
        public const int SMTP_Port = 0;
        public const string SMTP_Username = "";
        public const string SMTP_Password = "";

        public MySqlConnection connection; 

        public DataCredential()
        {
            connection = new MySqlConnection(connectionUri);
        }

        public bool IsSuccess(params object[] item)
        {
            bool success = true;
            foreach (object x in item)
            {
                if(success)
                {
                    if (x is int)
                        success = (int)x > 0;
                    else if (x is string)
                        success = !x.ToString().IsEmptyOrAllSpaces() && x != null;
                    else if (x is decimal)
                        success = (decimal)x > 0;
                    else if (x is IEnumerable<object>)
                        success = (x as IEnumerable<object>).ToList().Count > 0;
                    else
                        success = x != null;
                }
            }
            return success;
            //if (item.All(x => x is int))
            //    return item.All(x => (int)x > 0);
            //else if (item[0] is string)
            //    return item.All(x => !x.ToString().IsEmptyOrAllSpaces() && x != null);
            //else if (item[0] is decimal)
            //    return item.All(x => (decimal)x > 0);
            //else
            //    return item.All(x => x != null);
        }
        public bool IsSuccess(params string[] item)
        {
            return item.All(x => !x.IsEmptyOrAllSpaces() && x != null);
        }   
    }

}
