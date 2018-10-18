////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserInfo.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/18/2018
 * Last Modified: 
 * Description: Model for UserInfo returned on login - to add Id as cookie for use in Request POST operations
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

namespace SignRequestExpress.Models.ResponseModels
{
    public class UserInfo : Resource
    {
        public Guid Id { get; set; }

        public string Sub { get; set; }

        public string Given_Name { get; set; }

        public string Family_Name { get; set; }
    }
}
