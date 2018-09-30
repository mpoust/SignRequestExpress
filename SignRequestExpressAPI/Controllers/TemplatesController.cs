////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: TemplatesController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/17/2018
 * Last Modified: 
 * Description: This controller will return data requested for Templates within the database.
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
    [ApiController]
    [ApiVersion("1.0")]
    public class TemplatesController : Controller
    {
        private readonly ITemplateService _templateService;
        private readonly PagingOptions _defaultPagingOptions;

        public TemplatesController(
            ITemplateService templateService,
            IOptions<PagingOptions> defaultPagingOptions)
        {
            _templateService = templateService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet("{templateId}", Name = nameof(GetTemplateByIdAsync))]
        public async Task<IActionResult> GetTemplateByIdAsync(Guid templateId, CancellationToken ct)
        {
            var template = await _templateService.GetTemplateAsync(templateId, ct);
            if (template == null) return NotFound();
            return Ok(template);
        }

        [HttpGet(Name = nameof(GetTemplatesAsync))]
        public async Task<IActionResult> GetTemplatesAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Template, TemplateEntity> sortOptions,
            [FromQuery] SearchOptions<Template, TemplateEntity> searchOptions,
            CancellationToken ct)
        {
            // Verify model state valid - based off of properties in PagingOptions
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            // Handling case if PagingOptions are omitted and null
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var templates = await _templateService.GetTemplatesAsync(
                pagingOptions,
                sortOptions,
                searchOptions,
                ct);

           // var collectionLink = Link.ToCollection(nameof(GetTemplatesAsync));

            var collection = PagedCollection<Template>.Create(
                Link.ToCollection(nameof(GetTemplatesAsync)),
                templates.Items.ToArray(),
                templates.TotalSize,
                pagingOptions);

            return Ok(collection);
        }
    }
}
