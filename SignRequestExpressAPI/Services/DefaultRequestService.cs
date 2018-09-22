﻿/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        public async Task<Guid> CreateRequestAsync(
            Guid userId, // Will use for validation later
            string reason,
            byte status,
            //DateTime requestedDate, // Can probably remove
            DateTime neededDate,
            //Guid approval,
            bool isProofNeeded,
            byte mediaFK,
            byte quantity,
            bool isVertical,
            short heightInch,
            short widthInch,
            Guid template,
            string information,
            string dataFileURI,
            string imageURI,
            //DateTime ModifiedDateTime,
            CancellationToken ct
            )
        {
            // TODO - probably create the logic of adding entities to other tables here too
            // calling services from those ones

            // Create the RequestEntity and add to context
            var id = Guid.NewGuid();

            var newRequest = _context.Request.Add(new RequestEntity
            {
                //TODO generate RequestNumber properly
                Id = id,
                RequestNumber = null,
                Status = 0,
                RequestedDate = DateTime.Now,
                NeededDate = neededDate,
                ApprovalFK = Guid.NewGuid(), // TODO add this ID to the Account table to prepare approval
                IsProofNeeded = isProofNeeded,
                MediaFK = mediaFK,
                Quantity = quantity,
                IsVertical = isVertical,
                HeightInch = heightInch,
                WidthInch = widthInch,
                TemplateFK = template,
                Information = information,
                DataFileURI = dataFileURI,
                ImageURI = imageURI,
                // RequestImageURI // TODO add functionality to generate connected to blob
                ModifiedDateTime = DateTime.Now
            });

            var created = await _context.SaveChangesAsync(ct);
            if (created < 1) throw new InvalidOperationException("Could not create the request");

            return id;
        }

        
    }
}