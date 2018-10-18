////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserinfoResponse.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/01/2018
 * Last Modified: 10/18/2018
 * Description: Model for the information returned with user info
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using AspNet.Security.OpenIdConnect.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public class UserinfoResponse : Resource
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.Subject)]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.GivenName)]
        public string GivenName { get; set; }

        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.FamilyName)]
        public string FamilyName { get; set; }
    }
}
