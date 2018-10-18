////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserinfoController.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/01/2018
 * Last Modified: 10/18/2018
 * Description: This controller will return the user info for current user
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignRequestExpressAPI.Models;
using SignRequestExpressAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Controllers
{
    [Route("/[controller]")]
    [Authorize]
    [ApiController]
    public class UserinfoController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserinfoController(IUserService userService)
        {
            _userService = userService;
        }

        // GET /userinfo
        [HttpGet(Name = nameof(Userinfo))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UserinfoResponse>> Userinfo()
        {
            var user = await _userService.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user does not exist."
                });
            }

            var userId = await _userService.GetUserIdAsync(User);

            return new UserinfoResponse
            {
                Id = (Guid) userId,
                Self = Link.To(nameof(Userinfo)),
                GivenName = user.FirstName,
                FamilyName = user.LastName,
                Subject = Url.Link(
                    nameof(UsersController.GetUserById),
                    new { userId })
            };
        }
    }
}
