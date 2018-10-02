using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extensions
{
   public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException($"{nameof(input)} cannot be null!");
            }
            string text = input.ToLower();
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
