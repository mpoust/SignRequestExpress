////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: SearchableByteAttribute.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 11/07/2018
 * Last Modified: 
 * Description: Extending the base searchable attribute.  Used to search for bytes.
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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchableByteAttribute : SearchableAttribute
    {
        public SearchableByteAttribute()
        {
            ExpressionProvider = new ByteSearchExpressionProvider();
        }
    }
}
