/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultRequestService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 9/21/2018
 * Description: This class implements IRequestService
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
    public class DefaultRequestService : IRequestService
    {
        private readonly SignAPIContext _context;

        public DefaultRequestService(SignAPIContext context)
        {
            _context = context;
        }

        public async Task<Request> GetRequestAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Request.SingleOrDefaultAsync(r => r.Id == id, ct);
            if (entity == null) return null;
            return Mapper.Map<Request>(entity);
        }

        public async Task<PagedResults<Request>> GetRequestsAsync(
            PagingOptions pagingOptions,
            SortOptions<Request, RequestEntity> sortOptions,
            SearchOptions<Request, RequestEntity> searchOptions,
            CancellationToken ct)
        {
            IQueryable<RequestEntity> query = _context.Request;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var allRequests = await query
                .ProjectTo<Request>()
                .ToListAsync();

            var pagedRequests = allRequests
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value);

            return new PagedResults<Request>
            {
                Items = pagedRequests,
                TotalSize = allRequests.Count
            };
        }
    }
}
