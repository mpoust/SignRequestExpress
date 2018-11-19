////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: AccountsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 10/23/2018
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

using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuthorizationService _authzService;

        public AccountsController(
            IAccountService accountService,
            IAuthorizationService authorizationService)
        {
            _accountService = accountService;
            _authzService = authorizationService;
        }

        [Authorize]
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

            var canViewAllAccounts = await _authzService.AuthorizeAsync(
                User, "ViewAllAccountsPolicy");
            if (!canViewAllAccounts.Succeeded) return NotFound();
            else
            {
                return Ok(collection);
            }
        }

        [Authorize]
       // [Route("sales/{userId}")]
        [HttpGet("sales/{userId}", Name = nameof(GetUserAccountsAsync))]
        public async Task<IActionResult> GetUserAccountsAsync(Guid userId, CancellationToken ct)
        {
            var salesAccounts = await _accountService.GetUserAccountsAsync(userId, ct);

            var collectionLink = Link.ToCollection(nameof(GetUserAccountsAsync));

            var collection = new Collection<Account>
            {
                Self = collectionLink,
                Value = salesAccounts.ToArray()
            };

            /*
            var canViewSalesAccounts = await _authzService.AuthorizeAsync(
                User, "ViewSalesAccountsPolicy");
            if (!canViewSalesAccounts.Succeeded) return NotFound();
            else
            {
                return Ok(collection);
            }
            */

            return Ok(collection);
        }

        // /accounts/{accountId}
        [Authorize]
        [HttpGet("{accountId}", Name = nameof(GetAccountByIdAsync))]
        public async Task<IActionResult> GetAccountByIdAsync(Guid accountId, CancellationToken ct)
        {
            var account = await _accountService.GetAccountAsync(accountId, ct);
            if (account == null) return NotFound();
            return Ok(account);
        }
    }
}
