////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: StringExtensions.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/22/2018
 * Last Modified: 
 * Description: Allows for an input string to be converted. Only current method is ToCamelCase
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var first = input.Substring(0, 1).ToLower();
            if (input.Length == 1) return first;

            return first + input.Substring(1);
        }
    }
}
