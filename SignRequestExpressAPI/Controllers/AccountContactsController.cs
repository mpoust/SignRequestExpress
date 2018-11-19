////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: AccountContactsController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified: 
 * Description: This controller will return data requested for AccountContacts within the database.
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
    public class AccountContactsController : ControllerBase
    {
        private readonly IAccountContactService _accountContactService;

        public AccountContactsController(IAccountContactService accountContactService)
        {
            _accountContactService = accountContactService;
        }

        [Authorize]
        [HttpGet(Name = nameof(GetAccountContactsAsync))]
        public async Task<IActionResult> GetAccountContactsAsync(CancellationToken ct)
        {
            var accountContacts = await _accountContactService.GetAccountContactsAsync(ct);

            var collectionLink = Link.ToCollection(nameof(GetAccountContactsAsync));

            var collection = new Collection<AccountContact>
            {
                Self = collectionLink,
                Value = accountContacts.ToArray()
            };

            return Ok(collection);
        }

        [Authorize]
        [HttpGet("{accountContactId}", Name = nameof(GetAccountContactByIdAsync))]
        public async Task<IActionResult> GetAccountContactByIdAsync(int accountContactId, CancellationToken ct)
        {
            var accountContact = await _accountContactService.GetAccountContactAsync(accountContactId, ct);
            if (accountContact == null) return NotFound();
            return Ok(accountContact);
        }
    }
}
