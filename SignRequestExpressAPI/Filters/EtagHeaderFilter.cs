////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: EtagHeaderFilter.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/23/2018
 * Last Modified: 
 * Description: 
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SignRequestExpressAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Filters
{
    public class EtagHeaderFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Features.Set<IEtagHandlerFeature>(
                new EtagHandlerFeature(context.HttpContext.Request.Headers));

            var executed = await next();

            var result = executed?.Result as ObjectResult;

            var etag = (result?.Value as IEtaggable)?.GetEtag();
            if (string.IsNullOrEmpty(etag)) return;

            if (!etag.Contains('"'))
            {
                etag = $"\"{etag}\"";
            }

            context.HttpContext.Response.Headers.Add("ETag", etag);

            // If a response body was set so that we would add
            // the ETag header, but the status code is 304,
            // the body should be removed.
            if (result.StatusCode == 304)
            {
                result.Value = null;
            }

            return;
        }
    }
}
