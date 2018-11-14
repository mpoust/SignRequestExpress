/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultRequestService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 11/13/2018
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

        public async Task<PagedResults<Request>> GetUserRequestsAsync(
            Guid? userId,
            PagingOptions pagingOptions,
            SortOptions<Request, RequestEntity> sortOptions,
            SearchOptions<Request, RequestEntity> searchOptions,
            CancellationToken ct)
        {
            // Get Requests where Request.ID IN User_Request where userFK is userId
            if (userId == null) return null;

            //IQueryable<RequestEntity> query = from r in _context.Request
            //                                  where r.Id ;
            //query.FromSql($"SELECT * FROM Request AS r WHERE r.ID IN(SELECT ur.requestFK FROM User_Request AS ur WHERE ur.userFK = {userId})");
            // TODO: limit the requests to those that are the userID

            //query.

            IQueryable<RequestEntity> query = from r in _context.Request //where r.Status == 1 select r;
                                              join ur in _context.User_Request
                                              on r.Id equals
                                              ur.RequestFK
                                              where ur.UserFK == userId
                                              select r;

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
            decimal heightInch,
            decimal widthInch,
            Guid? template,
            string information,
            string dataFileURI,
            string imageURI,
            //DateTime ModifiedDateTime,
            CancellationToken ct
            )
        {
            // TODO - probably create the logic of adding entities to other tables here too -- create stored procedure instead
            // calling services from those ones

            // Create the RequestEntity and add to context
            var id = Guid.NewGuid(); // This is the RequestID

            // TODO: Generate entries for other tables associated with the request - tie to user and account

            var newRequest = _context.Request.Add(new RequestEntity
            {
                //TODO generate RequestNumber properly
                Id = id,
                Reason = reason,
                RequestNumber = null,
                Status = 0, // 0 is 'Submitted, waiting for approval'
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

        public async Task DeleteRequestAsync(Guid requestId, CancellationToken ct)
        {
            var request = await _context.Request
                .SingleOrDefaultAsync(r => r.Id == requestId, ct);
            if (request == null) return;

            _context.Request.Remove(request);
            await _context.SaveChangesAsync();
        }

        // Used to return Template ID from provided Blob URI on Request Submit
        public async Task<Guid?> GetTemplateIdAsync(string Uri, CancellationToken ct)
        {
            var template = await _context.Template
                .SingleOrDefaultAsync(t => t.ImageURI == Uri, ct);
            if (template == null) return null;

            return template.Id;
        }

        
    }
}
