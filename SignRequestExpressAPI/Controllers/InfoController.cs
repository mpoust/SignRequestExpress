////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: InfoController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 9/22/2018
 * Description: This controller returns the static data provided for the company from appsettings.json that is created into a 
 *  CompanyInfo object for IOptions in the service container. 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignRequestExpressAPI.Infrastructure;
using SignRequestExpressAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Controllers
{
    [Route("/[controller]")]
    [ApiVersion("1.0")]
    public class InfoController : Controller
    {
        private readonly CompanyInfo _companyInfo;  

        // Inject CompanyInfo into controller with constructor injection
        public InfoController(IOptions<CompanyInfo> companyInfoAccessor)
        {
            _companyInfo = companyInfoAccessor.Value;
        }

        [HttpGet(Name =nameof(GetInfo))]
        [ResponseCache(CacheProfileName = "Static")] // 1 Day cache
        [Etag]
        public IActionResult GetInfo()
        {
            _companyInfo.Href = Url.Link(nameof(GetInfo), null);

            if (!Request.GetEtagHandler().NoneMatch(_companyInfo))
            {
                return StatusCode(304, _companyInfo);
            }

            return Ok(_companyInfo);
        }
    }
}
