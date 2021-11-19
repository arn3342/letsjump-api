using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetsJump.DataAccess.Security
{
    public class TokenManager
    {
        private static int startLength = 3;
        public static string GenerateAccessToken(int id)
        {
            #region Setting the basic rules
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$%^&*()!@#";
            Random random = new Random();
            int totalLenth = 20;
            #endregion

            //Generating first 3 random characters
            string first3 = new string(Enumerable.Range(1, startLength).Select(_ => chars[random.Next(chars.Length)]).ToArray());
            //Generating a random number
            int countToLookAfter = random.Next(0, 9);
            //Getting the length of userID
            int idLength = id.ToString().Length;
            //Generating a random string where length = countToLookAfter
            string nextFew = new string(Enumerable.Range(1, countToLookAfter).Select(_ => chars[random.Next(chars.Length)]).ToArray());
            //Finally building the string with userID
            string uniqueString = first3 + countToLookAfter.ToString() + idLength.ToString() + nextFew + id.ToString();
            //Creating a random string of the remaining length
            int leftLength = totalLenth - uniqueString.Length;
            string finalRandom = "";
            if (leftLength > 0)
            {
                finalRandom = new string(Enumerable.Range(1, leftLength).Select(_ => chars[random.Next(chars.Length)]).ToArray());
            }

            string AccessToken = uniqueString + finalRandom;

            return AccessToken;
        }

        /// <summary>
        /// Deciphers the accesstoken based on conditions : Remove first 3 characters, pick the 4th number from string, find the occurance of ID based on the previous number selected.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static int DecipherID(string accessToken)
        {
            int idPositionStart = (int)char.GetNumericValue(accessToken[3]);
            int startIndex = startLength + 1 + idPositionStart + idPositionStart.ToString().Length;

            int idPositionEnd = startIndex + (int)char.GetNumericValue(accessToken[4]);

            int idLength = idPositionEnd - startIndex;
            int ID = int.TryParse(accessToken.Substring(startIndex, idLength), out int outValue) ? outValue : default(int);

            return ID;
        }

        public static bool IsAdminToken(string token)
        {
            return token == "iamtheadminaousaf";
        }
    }
}
