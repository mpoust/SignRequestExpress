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
using SignRequestExpressAPI.Entities;
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

        public async Task<Guid> GetTemplateBrandKeyAsync(Guid? templateId, CancellationToken ct)
        {
            var entity = await _context.Template.SingleOrDefaultAsync(t => t.Id == templateId, ct);

            return entity.BrandFK;
        }

        
        public async Task<Template> GetTemplateAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Template.SingleOrDefaultAsync(t => t.Id == id, ct);
            if (entity == null) return null;
            return Mapper.Map<Template>(entity);
        }

        public async Task<PagedResults<Template>> GetTemplatesAsync(
            PagingOptions pagingOptions, 
            SortOptions<Template, TemplateEntity> sortOptions,
            SearchOptions<Template, TemplateEntity> searchOptions,
            CancellationToken ct)
        {
            IQueryable<TemplateEntity> query = _context.Template;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var allTemplates = await query
                .ProjectTo<Template>()
                .ToListAsync();

            var pagedTemplates = allTemplates
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value);

            return new PagedResults<Template>
            {
                Items = pagedTemplates,
                TotalSize = allTemplates.Count
            };
        }
    }
}
