using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Extensions
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string str) 
        { 
            if (string.IsNullOrEmpty(str)) return str;

            return str.Substring(0, 1).ToUpperInvariant() + str.Substring(1).ToLowerInvariant();
        }

        public static bool IsInteger(this string str)
        {
            return int.TryParse(str, out _);
        }
        public static bool IsDouble(this string str)
        {
            return double.TryParse(str, out _);
        }
    }
}
