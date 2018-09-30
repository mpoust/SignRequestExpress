////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: IUserService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/17/2018
 * Last Modified:
 * Description: This service wraps User data access to keep the UsersController as thin as possible. This way we rely on a service to 
 *  interact with the DB context and separate the data access concerns from the controller. This has the added benefit of making the controller
 *  easier to test because we can inject a mock service instead of having to mock the entire database context. 
 *  
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Models;

using System;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Services
{
    public interface IUserService
    {
        Task<PagedResults<User>> GetUsersAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions);

        Task<(bool Succeeded, string ErrorMessage)> CreatUserAsync(RegisterForm form);

        //Task<Guid?> GetUserIdAsync(ClaimsPrincipal principal);

        Task<User> GetUserByIdAsync(Guid userId);

        //Task<User> GetUserAsync(ClaimsPrincipal user);
    }
}
