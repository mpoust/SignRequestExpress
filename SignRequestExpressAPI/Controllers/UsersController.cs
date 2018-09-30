////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UsersController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/17/2018
 * Last Modified: 9/30/2018
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignRequestExpressAPI.Models;
using SignRequestExpressAPI.Infrastructure;
using SignRequestExpressAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SignRequestExpressAPI.Entities;

namespace SignRequestExpressAPI.Controllers
{
    [Route("/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly PagingOptions _defaultPagingOptions;
        //private readonly IAuthorizationService _authzService;

        public UsersController(
            IUserService userService,
            IOptions<PagingOptions> defaultPagingOptions)
        {
            _userService = userService;
            _defaultPagingOptions = defaultPagingOptions.Value;
           // _authzService = AuthorizationService;
        }

        [HttpGet(Name = nameof(GetVisibleUsers))]
        public async Task<ActionResult<PagedCollection<User>>> GetVisibleUsers(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions,
            [FromQuery] SearchOptions<User, UserEntity> searchOptions)
        {
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            // TODO: Authorization check. Is the user an executive or admin?

            // TODO: return a collection of visible users
            var users = await _userService.GetUsersAsync(
                pagingOptions, sortOptions, searchOptions);

            var collection = PagedCollection<User>.Create(
                Link.ToCollection(nameof(GetVisibleUsers)),
                users.Items.ToArray(),
                users.TotalSize,
                pagingOptions);

            return collection;
        }

        //[Authorize]
        [HttpGet("{userId}", Name = nameof(GetUserById))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            throw new NotImplementedException();
        }

        /*
        // POST /users
        [HttpPost(Name = nameof(RegisterUser))]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterForm form)
        {
            throw new NotImplementedException();
        }
        */
    }
}
