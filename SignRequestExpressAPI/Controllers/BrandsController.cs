////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: BrandssController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified: 10/24/2018
 * Description: This controller will return data requested for Brands within the database.
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
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [Authorize]
        [HttpGet(Name = nameof(GetBrandsAsync))]
        public async Task<IActionResult> GetBrandsAsync(CancellationToken ct)
        {
            var brands = await _brandService.GetBrandsAsync(ct);

            var collectionLink = Link.ToCollection(nameof(GetBrandsAsync));

            var collection = new Collection<Brand>
            {
                Self = collectionLink,
                Value = brands.ToArray()
            };

            return Ok(collection);
        }

        [Authorize]
        [HttpGet("{brandId}", Name = nameof(GetBrandByIdAsync))]
        public async Task<IActionResult> GetBrandByIdAsync(Guid brandId, CancellationToken ct)
        {
            var brand = await _brandService.GetBrandAsyc(brandId, ct);
            if (brand == null) return NotFound();
            return Ok(brand);
        }
    }
}
