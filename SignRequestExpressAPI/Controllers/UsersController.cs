////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UsersController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/17/2018
 * Last Modified:
 * Description: This controller will return data requested for Users within the database.
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
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = nameof(GetUsersAsync))]
        public async Task<IActionResult> GetUsersAsync(CancellationToken ct)
        {
            var users = await _userService.GetUsersAsync(ct);

            var collectionLink = Link.ToCollection(nameof(GetUsersAsync));

            var collection = new Collection<User>
            {
                Self = collectionLink,
                Value = users.ToArray()
            };

            return Ok(collection);
        }

        [HttpGet("{userId}", Name = nameof(GetUserByIdAsync))]
        public async Task<IActionResult> GetUserByIdAsync(Guid userId, CancellationToken ct)
        {
            var user = await _userService.GetUserAsync(userId, ct);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
