////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: PagedCollection{T}.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified:
 * Description: Extending the base Collection model to return a collection of resources with the specified number per page of in the response
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
    public class PagedCollection<T> : Collection<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Offset { get; set; } // Offset this page has from the begging of the collection

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; } // How many items to return in this current page

        public int Size { get; set; } // Total number of items in entire collection, regardless of pagination. 

        // Navigation Properties:
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link First { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link Previous { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link Next { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link Last { get; set; }
    }
}
