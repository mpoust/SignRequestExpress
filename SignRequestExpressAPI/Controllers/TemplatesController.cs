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
    public class TemplatesController : Controller
    {
        private readonly ITemplateService _templateService;

        public TemplatesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet(Name = nameof(GetTemplatesAsync))]
        public async Task<IActionResult> GetTemplatesAsync(CancellationToken ct)
        {
            var templates = await _templateService.GetTemplatesAsync(ct);

            var collectionLink = Link.ToCollection(nameof(GetTemplatesAsync));

            var collection = new Collection<Template>
            {
                Self = collectionLink,
                Value = templates.ToArray()
            };

            return Ok(collection);
        }

        [HttpGet("{templateId}", Name = nameof(GetTemplateByIdAsync))]
        public async Task<IActionResult> GetTemplateByIdAsync(Guid templateId, CancellationToken ct)
        {
            var template = await _templateService.GetTemplateAsync(templateId, ct);
            if (template == null) return NotFound();
            return Ok(template);
        }
    }
}
