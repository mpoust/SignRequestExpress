////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RequestsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 11/13/2018
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
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authzService;
        private readonly IRequestService _requestService;
        private readonly PagingOptions _defaultPagingOptions;

        public RequestsController(
            IUserService userService,
            IAuthorizationService authorizationService,
            IRequestService requestService,
            IOptions<PagingOptions> defaultPagingOptions)
        {
            _userService = userService;
            _authzService = authorizationService;
            _requestService = requestService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        // Used for updating the status of the sign request
        [Authorize]
        [HttpPatch("{requestId}")]
        public async Task<IActionResult> PatchRequestStatus(
            [FromBody] JsonPatchDocument<Request> patchDocument,
            Guid requestId,
            CancellationToken ct)
        {
            // TODO: Add Admin or SignShop authorization only

            if(patchDocument == null)
            {
                return BadRequest();
            }

            // Get Request object from database based on ID
            var request = await _requestService.GetRequestAsync(requestId, ct);
            if (request == null) return NotFound();

            // Apply changes to Request object
            patchDocument.ApplyTo(request);
            
            await _requestService.UpdateRequestAsync(request, requestId, ct);

            return Ok(request);
        }

        // GET /requests/{requestId}
        [Authorize]
        [HttpGet("{requestId}", Name = (nameof(GetRequestByIdAsync)))]
        public async Task<IActionResult> GetRequestByIdAsync(Guid requestId, CancellationToken ct)
        {
            var request = await _requestService.GetRequestAsync(requestId, ct);
            if (request == null) return NotFound();
            return Ok(request);
        }


        //TODO: Rescrict who can see requests by role?
        // GET /requests
        [Authorize]
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

            //  var collectionLink = Link.ToCollection(nameof(GetRequestsAsync));

            // Logic if user is Sales then only return theirs, else return all for Admin and Sign Shop
            if (User.Identity.IsAuthenticated)
            {
                var canSeeAll = await _authzService.AuthorizeAsync(
                    User, "ViewAllRequestsPolicy");
                if (canSeeAll.Succeeded) // User is Administrator or Sign Shop
                {
                    var requests = await _requestService.GetRequestsAsync(
                                    pagingOptions,
                                    sortOptions,
                                    searchOptions,
                                    ct);

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
                else // User is Sales (Could technically be any other authenticated user)
                {
                    // Build collection only with requests for the User
                    var currentUserId = await _userService.GetUserIdAsync(User);

                    var requests = await _requestService.GetUserRequestsAsync(
                                        currentUserId,
                                        pagingOptions,
                                        sortOptions,
                                        searchOptions,
                                        ct);

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
            }

            // User is not authenticated.
            return NotFound();           
        }


        // POST /requests/{requestNumber}
        //[HttpPost("{requestNumber}", Name = nameof(SubmitRequestAsync))]
        [Authorize]
        [HttpPost(Name = nameof(SubmitRequestAsync))]
        public async Task<IActionResult> SubmitRequestAsync(
            //string requestNumber,
            [FromBody] RequestForm requestForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            // Business logic for creating a sign request
            // Get Template GUID from the string here, then pass the guid to create request
            Guid? templateId = await _requestService.GetTemplateIdAsync(requestForm.Template, ct);

            // Quick check on needed date
            if (!requestForm.NeededDate.HasValue) requestForm.NeededDate = DateTime.Now.AddDays(14);

            var requestId = await _requestService.CreateRequestAsync(
                    requestForm.UserId, 
                    requestForm.Account,
                    requestForm.Reason, 
                    0, 
                    requestForm.NeededDate.Value,
                    requestForm.IsProofNeeded.Value,
                    requestForm.MediaFK.Value,
                    requestForm.Quantity.Value,
                    requestForm.IsVertical.Value,
                    requestForm.HeightInch.Value,
                    requestForm.WidthInch.Value,
                    templateId,
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
        [Authorize]
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
