////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: PagingOptions.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified:
 * Description: Model to represent parameters necessary for specifing pagination options. Will bind automatically to certain
 *  calls from controllers.
 *  
 *  Starting with TemplatesController - potentially adding more as design proves necessary.
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public class PagingOptions
    {
        [Range(1, 9999, ErrorMessage = "Offset must be greater than 0.")]
        public int? Offset { get; set; }

        [Range(1, 100, ErrorMessage = "Limit must be greater than 0 and less than 100.")]
        public int? Limit { get; set; }

        public PagingOptions Replace(PagingOptions newer)
        {
            return new PagingOptions
            {
                Offset = newer.Offset ?? Offset,
                Limit = newer.Limit ?? Limit
            };
        }
    }
}
