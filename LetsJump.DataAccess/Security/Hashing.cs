using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCrypt.Net;
namespace LetsJump.DataAccess.Security
{
    public static class Hashing
    {
        /// <summary>
        /// This methods Generate a Random salt for the password 
        /// that we use for the enhancing the password
        /// </summary>
        /// <returns>Random Salt</returns>
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(6);
        }

        /// <summary>
        /// This method hash a plain text to a hash password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns> Hashed Password</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        /// <summary>
        /// This method Checks wheather the password matches with the stored hashed password in the database.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="correctHash"></param>
        /// <returns>true(if the user is authenticated) or false(when the user is not authenticated)</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }

        public static string GeneratePassword(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var list = Enumerable.Repeat(0, length).Select(x => chars[random.Next(chars.Length)]);
            return string.Join("", list);
        }
    }

}