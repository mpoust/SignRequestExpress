////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: User.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 9/30/2018
 * Description: Resource the API will return to the client - corresponding with UserEntity.cs
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using SignRequestExpressAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public class User : Resource
    {
        [Sortable(Default = true)]
        public short UserNumber { get;  set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

       // public byte RoleFK { get; set; }

        public bool IsEmailPreferred { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedDateTime { get; set; }

    }
}
