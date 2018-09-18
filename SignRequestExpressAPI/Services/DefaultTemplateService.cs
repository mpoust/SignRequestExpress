////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultTemplateService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/17/2018
 * Last Modified:
 * Description: This class implements ITemplateService.
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
    public class DefaultTemplateService : ITemplateService
    {
        private readonly SignAPIContext _context;

        public DefaultTemplateService(SignAPIContext context)
        {
            _context = context;
        }

        public async Task<Template> GetTemplateAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Account.SingleOrDefaultAsync(t => t.Id == id, ct);
            if (entity == null) return null;
            return Mapper.Map<Template>(entity);
        }

        public async Task<IEnumerable<Template>> GetTemplatesAsync(CancellationToken ct)
        {
            var query = _context.Template
                .ProjectTo<Template>();

            return await query.ToArrayAsync();
        }
    }
}
