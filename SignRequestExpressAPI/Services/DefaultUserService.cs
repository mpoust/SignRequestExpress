////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultAccountService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified:
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
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SignRequestExpressAPI.Models;

namespace SignRequestExpressAPI.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly SignAPIContext _context;

        public DefaultUserService(SignAPIContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.User.SingleOrDefaultAsync(u => u.Id == id, ct);
            if (entity == null) return null;
            return Mapper.Map<User>(entity);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken ct)
        {
            var query = _context.Account
                .ProjectTo<User>();

            return await query.ToArrayAsync();
        }
    }
}
