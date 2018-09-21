////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RootController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 9/20/2018
 * Description: Serves as the starting point of the API. Controller uses route attributes to tell the routing system which routes to handle.
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

namespace SignRequestExpressAPI.Controllers
{
    [Route("/")]
    [ApiVersion("1.0")]
    public class RootController : Controller
    {
        // IActionResult gives flexibility to return HTTP status codes, JSON responses, or both.
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new RootResponse
            {
                Self = Link.To(nameof(GetRoot)),
                Info = Link.To(nameof(InfoController.GetInfo)),
                Accounts = Link.To(nameof(AccountsController.GetAccountsAsync)),
                AccountContacts = Link.To(nameof(AccountContactsController.GetAccountContactsAsync)),
                Templates = Link.To(nameof(TemplatesController.GetTemplatesAsync)),
                Users = Link.To(nameof(UsersController.GetUsersAsync)),
                Brands = Link.To(nameof(BrandsController.GetBrandsAsync)),
                BrandStandards = Link.To(nameof(BrandStandardsController.GetBrandStandardsAsync)),
                Requests = Link.To(nameof(RequestsController.GetRequestsAsync))
            };

            return Ok(response);
        }
    }
}
