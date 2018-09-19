////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: SearchTerm.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified: 
 * Description: Holds the search term that is validated within SearchOptionsProcessor{T, TEntity}
 *  Represents a deconstructed search term
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
    public class SearchTerm
    {
        public string Name { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }

        public bool ValidSyntax { get; set; }
    }
}
