////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultAccountService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified:
 * Description: This class implements IAccountService.
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
    public class DefaultAccountService : IAccountService
    {
        private readonly SignAPIContext _context;

        public DefaultAccountService(SignAPIContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Account.SingleOrDefaultAsync(a => a.Id == id, ct);
            if (entity == null) return null;
            // if found, map entity properties into the account resource
            return Mapper.Map<Account>(entity);
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(CancellationToken ct)
        {
            var query = _context.Account
                .ProjectTo<Account>();

            return await query.ToArrayAsync();
        }

        // Used for populating the dropdown Accounts listing in the Sales Request Page in SPA
        public async Task<IEnumerable<Account>> GetUserAccountsAsync(Guid userId, CancellationToken ct)
        {
            // Get the Accounts for the UserID supplied
            var query = _context.Account
                .Where(a => a.AssociateFK == userId)
                .ProjectTo<Account>();

            return await query.ToArrayAsync();
        }
    }
}
