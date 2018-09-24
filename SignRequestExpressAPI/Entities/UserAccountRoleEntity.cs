////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserAccountRoleEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/24/2018
 * Last Modified:
 * Description: This entity gives roles to the UserAccountEntity (Admin, Sales, Sign Shop)
 *  
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
    public class UserAccountRoleEntity : IdentityRole<Guid>
    {
        public UserAccountRoleEntity()
            : base()
        { }

        public UserAccountRoleEntity(string roleName)
            : base(roleName)
        { }
    }
}
