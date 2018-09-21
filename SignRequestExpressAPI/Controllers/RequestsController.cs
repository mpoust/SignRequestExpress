////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RequestsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 
 * Description: This controller will return data requested for Requests within the database.
 * 
 * Note: CancellationTokens are included because ASP.NET Core automatically sends a cancellation mesage if the browser or client
 *  cancels the request unexpectedly. 
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Models;
using SignRequestExpressAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Controllers
{
    [Route("/[controller]")]
    [ApiVersion("1.0")]
    public class RequestsController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly PagingOptions _defaultPagingOptions;

        public RequestsController(
            IRequestService requestService,
            IOptions<PagingOptions> defaultPagingOptions)
        {
            _requestService = requestService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet("{requestId}", Name =(nameof(GetRequestByIdAsync)))]
        public async Task<IActionResult> GetRequestByIdAsync(Guid requestId, CancellationToken ct)
        {
            var request = await _requestService.GetRequestAsync(requestId, ct);
            if (request == null) return NotFound();
            return Ok(request);
        }

        public async Task<IActionResult> GetRequestsAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Request, RequestEntity> sortOptions,
            [FromQuery] SearchOptions<Request, RequestEntity> searchOptions,
            CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
