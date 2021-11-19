using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetsJump.DataAccess.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmptyOrAllSpaces(this string str)
        {
            return null != str && str.All(c => c.Equals(' '));
        }

        public static string ReplaceLastOccurrence(this string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
    }
}
