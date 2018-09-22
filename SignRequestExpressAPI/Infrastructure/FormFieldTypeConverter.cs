////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: FormFieldTypeConverter.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/22/2018
 * Last Modified: 
 * Description: 
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

namespace SignRequestExpressAPI.Infrastructure
{
    /// <summary>
    /// Converts some .NET types to ION Form Field types
    /// (http://ionwg.org/#form-field-type-values).
    /// </summary>
    public static class FormFieldTypeConverter
    {
        private static IReadOnlyDictionary<Type, string> TypeMapping = new Dictionary<Type, string>()
        {
            [typeof(bool)] = "boolean",
            [typeof(DateTime)] = "datetime",
            [typeof(DateTimeOffset)] = "datetime",
            [typeof(byte)] = "byte",
            [typeof(float)] = "decimal",
            [typeof(double)] = "decimal",
            [typeof(decimal)] = "decimal",
            [typeof(TimeSpan)] = "duration",
            [typeof(short)] = "integer",
            [typeof(int)] = "integer",
            [typeof(long)] = "integer",
            [typeof(string)] = "string"
        };

        public static string GetTypeName(Type fieldType)
        {
            if (fieldType.IsArray) return "array";

            // Unwrap Nullable<> if applicable
            var type = Nullable.GetUnderlyingType(fieldType) ?? fieldType;

            if (TypeMapping.TryGetValue(type, out var value)) return value;

            return null;
        }
    }
}
