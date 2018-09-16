////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Link.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 09/16/2018
 * Last Modified:
 * Description: This model represents a link that can be used outside of a controller to represent links between resources.
 *  This link will follow the Ion specification.
 *  
 *  Href will be first, Method will be ignored if null and if default of GET
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public class Link
    {
        public const string GetMethod = "GET";

        // Static helper method that returns a link with some default values
        public static Link To(string routeName, object routeValues = null)
            => new Link
            {
                RouteName = routeName,
                RouteValues = routeValues,
                Method = GetMethod,
                Relations = null
            };

        [JsonProperty(Order = -4)]
        public string Href { get; set; }

        [JsonProperty(Order = -3, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }

        [JsonProperty(Order = -2, PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }

        // Stores the route name before being rewritten -- Will not be serialized, only used internally
        [JsonIgnore]
        public string RouteName { get; set; }

        // Stores the route name before being rewritten -- Will not be serialized, only used internally
        [JsonIgnore]
        public object RouteValues { get; set; }
    }
}
