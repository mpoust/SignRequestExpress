////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultBrandStandardsService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified:
 * Description: This class implements IBrandStandardsService.
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
    public class DefaultBrandStandardsService : IBrandStandardsService
    {
        private readonly SignAPIContext _context;

        public DefaultBrandStandardsService(SignAPIContext context)
        {
            _context = context;
        }

        public async Task<BrandStandards> GetBrandStandardAsync(int id, CancellationToken ct)
        {
            var entity = await _context.BrandStandards.SingleOrDefaultAsync(bs => bs.Id == id, ct);
            if (entity == null) return null;
            return Mapper.Map<BrandStandards>(entity);
        }

        public async Task<IEnumerable<BrandStandards>> GetBrandStandardsAsync(CancellationToken ct)
        {
            var query = _context.BrandStandards
                .ProjectTo<BrandStandards>();

            return await query.ToArrayAsync();
        }
    }
}
