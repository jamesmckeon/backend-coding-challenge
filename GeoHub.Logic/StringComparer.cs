using System;
using System.Collections.Generic;

namespace GeoHub.Logic
{
    /// <summary>
    /// Implements case and whitespace  insensitive string comparisons
    /// </summary>
    public class StringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {

            if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y))
            {
                return false;
            }
            else
            {
                return Fix(x).Equals(Fix(y));
            }

        }

        protected string Fix(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return str.ToLower().Trim();
            }

        }
        public int GetHashCode(string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
             return   string.Empty.GetHashCode();
            }
            else
            {
                return Fix(obj).GetHashCode();
            }

        }
    }
}