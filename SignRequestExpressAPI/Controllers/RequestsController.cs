////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RequestsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 9/22/2018
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Infrastructure;
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
    [ApiController]
    [ApiVersion("1.0")]
    public class RequestsController : ControllerBase
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

        // GET /requests/{requestId}
        [HttpGet("{requestId}", Name = (nameof(GetRequestByIdAsync)))]
        public async Task<IActionResult> GetRequestByIdAsync(Guid requestId, CancellationToken ct)
        {
            var request = await _requestService.GetRequestAsync(requestId, ct);
            if (request == null) return NotFound();
            return Ok(request);
        }


        //TODO: Rescrict who can see requests by role?
        // GET /requests
        [HttpGet(Name = nameof(GetRequestsAsync))]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "offset", "limit", "orderBy", "search" })]
        public async Task<IActionResult> GetRequestsAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Request, RequestEntity> sortOptions,
            [FromQuery] SearchOptions<Request, RequestEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var requests = await _requestService.GetRequestsAsync(
                pagingOptions,
                sortOptions,
                searchOptions,
                ct);

          //  var collectionLink = Link.ToCollection(nameof(GetRequestsAsync));

            var collection = PagedCollection<Request>.Create<RequestResponse>(
                Link.ToCollection(nameof(GetRequestsAsync)),
                requests.Items.ToArray(),
                requests.TotalSize,
                pagingOptions);

            collection.RequestsQuery = FormMetadata.FromResource<Request>(
                Link.ToForm(
                    nameof(GetRequestsAsync),
                    null,
                    Link.GetMethod,
                    Form.QueryRelation));

            return Ok(collection);
        }


        // POST /requests/{requestNumber}
        //[HttpPost("{requestNumber}", Name = nameof(SubmitRequestAsync))]
        [Authorize]
        [HttpPost(Name = nameof(SubmitRequestAsync))]
        public async Task<IActionResult> SubmitRequestAsync(
            string requestNumber,
            [FromBody] RequestForm requestForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            // Business logic for creating a sign request

            // Quick check on needed date
            if (!requestForm.NeededDate.HasValue) requestForm.NeededDate = DateTime.Now.AddDays(14);

            var requestId = await _requestService.CreateRequestAsync(
                    requestForm.UserId, 
                    requestForm.Reason, 
                    0, // User Number TODO: Increment - this number is used for user visual and seeing sign requests
                    requestForm.NeededDate.Value,
                    requestForm.IsProofNeeded.Value,
                    requestForm.MediaFK.Value,
                    requestForm.Quantity.Value,
                    requestForm.IsVertical.Value,
                    requestForm.HeightInch.Value,
                    requestForm.WidthInch.Value,
                    requestForm.Template.Value,
                    requestForm.Information, 
                    requestForm.DataFileURI, 
                    requestForm.ImageURI, 
                    ct);

            return Created(
                Url.Link(nameof(RequestsController.GetRequestByIdAsync),
                new { requestId }),
                null);
        }


        // Delete Request by ID
        [HttpDelete("{requestId}", Name = nameof(DeleteRequestByIdAsync))]
        public async Task<IActionResult> DeleteRequestByIdAsync(
            Guid requestId,
            CancellationToken ct)
        {
            // TODO: Authorize user is allowed to delete Request
            await _requestService.DeleteRequestAsync(requestId, ct);
            return NoContent();
        }

        // Delete Request by RequestNum??
    }
}
