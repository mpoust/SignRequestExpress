////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserRoleEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/30/2018
 * Last Modified: 
 * Description: This entity models a role a user can be in. 
 *  Extends from IdentityRole
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
    public class UserRoleEntity : IdentityRole<Guid>
    {
        public UserRoleEntity()
            : base()
        {
        }

        public UserRoleEntity(string roleName)
            : base(roleName)
        {
        }
    }
}
