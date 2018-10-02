////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: AccountsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 9/16/2018
 * Description: This controller will return data requested for Accounts within the database.
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
using Microsoft.EntityFrameworkCore;
using SignRequestExpressAPI.Models;
using SignRequestExpressAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Controllers
{
    // "/accounts"
    [Route("/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet(Name = nameof(GetAccountsAsync))]
        public async Task<IActionResult> GetAccountsAsync(CancellationToken ct)
        {
            var accounts = await _accountService.GetAccountsAsync(ct);

            var collectionLink = Link.ToCollection(nameof(GetAccountsAsync));

            var collection = new Collection<Account>
            {
                Self = collectionLink,
                Value = accounts.ToArray()
            };

            return Ok(collection);
        }

        // /accounts/{accountId}
        [HttpGet("{accountId}", Name = nameof(GetAccountByIdAsync))]
        public async Task<IActionResult> GetAccountByIdAsync(Guid accountId, CancellationToken ct)
        {
            var account = await _accountService.GetAccountAsync(accountId, ct);
            if (account == null) return NotFound();
            return Ok(account);
        }
    }
}
