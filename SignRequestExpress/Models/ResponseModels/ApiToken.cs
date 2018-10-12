////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: ApiToken.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/12/2018
 * Last Modified: 
 * Description: Model for Token returned from API during AuthN.
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
    public class ApiToken : Resource
    {
        public string Scope { get; set; }

        public string Token_Type { get; set; }

        public string Access_Token { get; set; }

        public string Expires_In { get; set; }
    }
}
