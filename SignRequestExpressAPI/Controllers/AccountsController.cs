﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: AccountsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 
 * Description: 
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Controllers
{
    // "/accounts"
    [Route("/[controller]")]
    [ApiVersion("1.0")]
    public class AccountsController : Controller
    {
        [HttpGet(Name = nameof(GetAccounts))]
        public IActionResult GetAccounts()
        {
            throw new NotImplementedException();
        }
    }
}
