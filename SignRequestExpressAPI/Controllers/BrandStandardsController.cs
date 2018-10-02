////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: BrandStandardsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified: 
 * Description: This controller will return data requested for BrandStandards within the database.
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
    [ApiController]
    [ApiVersion("1.0")]
    public class BrandStandardsController : ControllerBase
    {
        private readonly IBrandStandardsService _brandStandardsService;

        public BrandStandardsController(IBrandStandardsService brandStandardsService)
        {
            _brandStandardsService = brandStandardsService;
        }

        [HttpGet(Name = nameof(GetBrandStandardsAsync))]
        public async Task<IActionResult> GetBrandStandardsAsync(CancellationToken ct)
        {
            var brandStandards = await _brandStandardsService.GetBrandStandardsAsync(ct);

            var collectionLink = Link.ToCollection(nameof(GetBrandStandardsAsync));

            var collection = new Collection<BrandStandards>
            {
                Self = collectionLink,
                Value = brandStandards.ToArray()
            };

            return Ok(collection);
        }

        [HttpGet("{brandStandardId}", Name = nameof(GetBrandStandardbyIdAsync))]
        public async Task<IActionResult> GetBrandStandardbyIdAsync(int brandStandardId, CancellationToken ct)
        {
            var brandStandard = await _brandStandardsService.GetBrandStandardAsync(brandStandardId, ct);
            if (brandStandard == null) return NotFound();
            return Ok(brandStandard);
        }
    }
}
