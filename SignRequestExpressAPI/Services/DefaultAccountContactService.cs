////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultAccountService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified:
 * Description: This class implements IAccountContactService.
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
    public class DefaultAccountContactService : IAccountContactService
    {
        private readonly SignAPIContext _context;

        public DefaultAccountContactService(SignAPIContext context)
        {
            _context = context;
        }

        public async Task<AccountContact> GetAccountContactAsync(int id, CancellationToken ct)
        {
            var entity = await _context.AccountContact.SingleOrDefaultAsync(a => a.Id == id, ct);
            if (entity == null) return null;
            return Mapper.Map<AccountContact>(entity);
        }

        public async Task<IEnumerable<AccountContact>> GetAccountContactsAsync(CancellationToken ct)
        {
            var query = _context.AccountContact
                .ProjectTo<AccountContact>();

            return await query.ToArrayAsync();
        }
    }
}
