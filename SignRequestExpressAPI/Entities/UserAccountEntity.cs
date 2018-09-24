////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserAccountEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/24/2018
 * Last Modified:
 * Description: This is an entity that models an object from the UserAccount table from the database allowing the user to be
 *  authenticated on login.
 *  
 *  Derives from IdentityUser which will have a Guid ID, and contains properties for UserName, PasswordHash, and more
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
    public class UserAccountEntity : IdentityUser<Guid>
    {
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public Guid UserFK { get; set; }
    }
}
