////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: LinkRewriter.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified:
 * Description: This class will create an absolute URL with the help of IUrlHelper for an instance of the Link class.
 * 
 * References:  
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SignRequestExpressAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Infrastructure
{
    public class LinkRewriter
    {
        private readonly IUrlHelper _urlHelper;

        public LinkRewriter(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        
        // This needs to be called right before a response is serialized and sent back to the client - achieved with a ResultFilter
        //  called LinkRewritingFilter
        public Link Rewrite(Link original)
        {
            if (original == null) return null;

            return new Link
            {
                Href = _urlHelper.Link(original.RouteName, original.RouteValues),
                Method = original.Method,
                Relations = original.Relations
            };
        }
    }
}
