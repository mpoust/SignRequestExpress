﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Resource.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 
 * Description: Abstract base class for all the resource models that will be created to return from the API.
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public abstract class Resource : Link
    {
        // Replacement to string Href property so Link rewriter will create proper self-referential links
        [JsonIgnore]
        public Link Self { get; set; } // absolute URL to resource
    }
}
