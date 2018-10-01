////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 9/30/2018
 * Description: This is an entity that models an object from the User table from the database.
 *  Extends IdentityUser from Microsoft Identity
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Entities
{
    public class UserEntity : IdentityUser<Guid>
    {
        public short UserNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public byte RoleFK { get; set; }

        public bool IsEmailPreferred { get; set; }

      //  public DateTime CreatedAt { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}
