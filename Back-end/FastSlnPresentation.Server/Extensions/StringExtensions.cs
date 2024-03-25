using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FastSlnPresentation.Server.Extensions
{
    public static class StringExtensions
    {
        public static string ToKebabCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
