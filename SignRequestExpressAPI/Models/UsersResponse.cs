////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UsersResponse.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/01/2018
 * Last Modified: 
 * Description: This response gets added to a request on a user resource
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
    public class UsersResponse : PagedCollection<User>
    {
        public Form Register { get; set; }

        public Link Me { get; set; }
    }
}
