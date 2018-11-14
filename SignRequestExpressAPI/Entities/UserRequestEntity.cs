////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserRequestEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 11/14/2018
 * Last Modified: 
 * Description:
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

namespace SignRequestExpressAPI.Entities
{
    public class User_RequestEntity
    {
        public Guid UserFK { get; set; }

        public Guid RequestFK { get; set; }
    }
}
