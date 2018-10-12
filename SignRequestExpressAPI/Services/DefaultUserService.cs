////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultAccountService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified: 10/12/2018
 * Description: This class implements IUserService.
 *  
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Models;

namespace SignRequestExpressAPI.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly SignAPIContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfigurationProvider _mappingConfiguration;

        public DefaultUserService(
            UserManager<UserEntity> userManager,
            IConfigurationProvider mappingConfiguration)
        {
            _userManager = userManager;
            _mappingConfiguration = mappingConfiguration;
        }

        public async Task<(bool Succeeded, string ErrorMessage)> CreateUserAsync(
            RegisterForm form)
        {
            var id = Guid.NewGuid();

            var entity = new UserEntity
            {
                Id = id,
                UserName = form.UserName,
                FirstName = form.FirstName,
                LastName = form.LastName,
                PhoneNumber = form.PhoneNumber,
                Email = form.Email,
                CreatedAt = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            };
            
            var result = await _userManager.CreateAsync(entity, form.Password); // Validates password here
            await _userManager.AddToRoleAsync(entity, form.Role);
            if (!result.Succeeded)
            {
                var firstError = result.Errors.FirstOrDefault()?.Description;
                return (false, firstError);
            }

            return (true, null);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            var entity = await _userManager.GetUserAsync(user);
            var mapper = _mappingConfiguration.CreateMapper();

            return mapper.Map<User>(entity);
        }

        public async Task<Guid?> GetUserIdAsync(ClaimsPrincipal principal)
        {
            var entity = await _userManager.GetUserAsync(principal);
            if (entity == null) return null;

            return entity.Id;
        }

        public async Task<PagedResults<User>> GetUsersAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions)
        {
            IQueryable<UserEntity> query = _userManager.Users;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync();

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>(_mappingConfiguration)
                .ToArrayAsync();

            return new PagedResults<User>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var entity = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Id == userId);
            var mapper = _mappingConfiguration.CreateMapper();

            return mapper.Map<User>(entity);
        }
    }
}
