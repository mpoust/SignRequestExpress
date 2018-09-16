////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RootResponse.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified:
 * Description: The root controller not returns a this response model. Contains Links from the Link model.
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

namespace SignRequestExpressAPI.Models
{
    public class RootResponse : Resource
    {
        public Link Info { get; set; }

        public Link Accounts { get; set; }
    }
}
