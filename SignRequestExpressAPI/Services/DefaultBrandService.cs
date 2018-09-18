////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultBrandService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified:
 * Description: This class implements IBrandService.
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
    public class DefaultBrandService : IBrandService
    {
        private readonly SignAPIContext _context;

        public DefaultBrandService(SignAPIContext context)
        {
            _context = context;
        }

        public async Task<Brand> GetBrandAsyc(Guid id, CancellationToken ct)
        {
            var entity = await _context.Brand.SingleOrDefaultAsync(b => b.Id == id, ct);
            if (entity == null) return null;
            return Mapper.Map<Brand>(entity);
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync(CancellationToken ct)
        {
            var query = _context.Brand
                .ProjectTo<Brand>();

            return await query.ToArrayAsync();
        }
    }
}
