/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: DefaultRequestService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 11/15/2018
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
        private readonly IAccountService _accountService;
        private readonly ITemplateService _templateService;

        public DefaultRequestService(
            SignAPIContext context,
            IAccountService accountService,
            ITemplateService templateService)
        {
            _context = context;
            _accountService = accountService;
            _templateService = templateService;
        }

        public async Task UpdateRequestAsync(Request request, Guid id, CancellationToken ct)
        {
            var entity = await _context.Request.SingleOrDefaultAsync(r => r.Id == id, ct);

            // Copy over values from modified request - will allow for PATCH to work on all properties of request
            entity.Reason = request.Reason;
            entity.Status = request.Status;
            entity.IsProofNeeded = request.IsProofNeeded;
            entity.MediaFK = request.MediaFK;
            entity.Quantity = request.Quantity;
            entity.HeightInch = request.HeightInch;
            entity.WidthInch = request.WidthInch;
            entity.TemplateFK = request.TemplateFK;
            entity.Information = request.Information;
            entity.ModifiedDateTime = DateTime.Now;

            _context.Request.Update(entity);
            await _context.SaveChangesAsync(ct);
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

            // TODO: rewrite queries and foreach to be more efficient.

            var accounts = from ra in _context.Request_Account
                           join a in _context.Account on ra.AccountFK equals a.Id
                           select new { RequestFK = ra.RequestFK, AccountName = a.AccountName};

            var associates = from ur in _context.User_Request
                             join u in _context.Users on ur.UserFK equals u.Id
                             select new { RequestFK = ur.RequestFK, FirstName = u.FirstName, LastName = u.LastName };

            var brands = from rb in _context.Request_Brand
                         join b in _context.Brand on rb.BrandFK equals b.Id
                         select new { RequestFK = rb.RequestFK, BrandName = b.BrandName };

            var allRequests = await query
                .ProjectTo<Request>()
                .ToListAsync();

            foreach (var request in allRequests)
            {
                foreach (var account in accounts)
                {
                    if (request.Id == account.RequestFK)
                    {
                        request.AccountName = account.AccountName;
                    }
                }

                foreach(var associate in associates)
                {
                    if(request.Id == associate.RequestFK)
                    {
                        request.AssociateName = associate.FirstName + " " + associate.LastName;                        
                    }
                }

                foreach(var brand in brands)
                {
                    if(request.Id == brand.RequestFK)
                    {
                        request.BrandName = brand.BrandName;
                    }
                }
            }

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
            if (userId == null) return null;

            IQueryable<RequestEntity> query = from r in _context.Request 
                                              join ur in _context.User_Request
                                              on r.Id equals
                                              ur.RequestFK
                                              where ur.UserFK == userId
                                              select r;

            var accounts = from ra in _context.Request_Account
                       join a in _context.Account on ra.AccountFK equals a.Id
                       select new { RequestFK = ra.RequestFK, AccountName = a.AccountName };

            var brands = from rb in _context.Request_Brand
                         join b in _context.Brand on rb.BrandFK equals b.Id
                         select new { RequestFK = rb.RequestFK, BrandName = b.BrandName };

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var allRequests = await query
                .ProjectTo<Request>()
                .ToListAsync();

            foreach(var request in allRequests)
            {
                foreach(var account in accounts)
                {
                    if(request.Id == account.RequestFK)
                    {
                        request.AccountName = account.AccountName;
                    }
                }

                foreach(var brand in brands)
                {
                    if(request.Id == brand.RequestFK)
                    {
                        request.BrandName = brand.BrandName;
                    }
                }
            }

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
            Guid userId,
            string account,
            string reason,
            byte status,
            DateTime neededDate,
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
            CancellationToken ct
            )
        {           
            var requestId = Guid.NewGuid(); 
            var approvalId = Guid.NewGuid();

            // TODO: refactor for those that don't have any requests yet
            // Get all Request Numbers for user
            IQueryable<string> query = from r in _context.Request 
                                              join ur in _context.User_Request
                                              on r.Id equals
                                              ur.RequestFK
                                              where ur.UserFK == userId
                                              select r.RequestNumber;

            // Creating and formatting RequestNumber for submission
            var requestNums = query.ToList();
            string requestNumber = "";
            if(requestNums.Count != 0)
            {
                requestNums.Sort();
                var last = requestNums.Last();
                var userNum = last.Substring(0, last.IndexOf('-'));
                var reqNum = int.Parse(last.Substring(last.LastIndexOf('-') + 1)) + 1;
                var year = DateTime.Now.ToString("yy");
                requestNumber = userNum + "-" + year + "-" + reqNum.ToString("0000");
            }
            else
            {
                // TODO: THIS NEEDS FIXED

                var userNum =  (from u in _context.Users
                                        where u.Id == userId
                                        select u.UserNumber).SingleOrDefault();
               // userNum = short.Parse(userNum);
                var year = DateTime.Now.ToString("yy");
                requestNumber = userNum + "-" + year + "-0001";
            }


            // Create the RequestEntity and add to context
            var newRequest = await _context.Request.AddAsync(new RequestEntity
            {
                //TODO generate RequestNumber properly
                Id = requestId,
                Reason = reason,
                RequestNumber = requestNumber,
                Status = 0, // 0 is 'Submitted, waiting for approval'
                RequestedDate = DateTime.Now,
                NeededDate = neededDate,
                ApprovalFK = approvalId,
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

            // Add entry into User_Request
            var newUserRequest =  _context.User_Request.Add(new User_RequestEntity
            {
                UserFK = userId,
                RequestFK = requestId
            });

            // Add entry to Approval
            var newApproval = _context.Approval.Add(new Approval
            {
                Id = approvalId,
                ModifiedDateTime = DateTime.Now,
                ApprovalStatus = 0,
                ApproverID = Guid.Parse("f5c1ef93-e013-4482-9369-0202ffe80c21") // DemoManager
            });

            // Add entry to Request_Account
            var accountId = await _accountService.GetAccountIdAsync(account, ct);
            var newRequestAccount = _context.Request_Account.Add(new Request_AccountEntity
            {
                RequestFK = requestId,
                AccountFK = accountId
            });

            // Add entry to Request_Brand
            var brandId = await _templateService.GetTemplateBrandKeyAsync(template, ct);
            var newRequestBrand = _context.Request_Brand.Add(new Request_BrandEntity
            {
                RequestFK = requestId,
                BrandFK = brandId
            });

            var created = await _context.SaveChangesAsync(ct);
            if (created < 1) throw new InvalidOperationException("Could not create the request");

            return requestId;
        }

        public async Task DeleteRequestAsync(Guid requestId, CancellationToken ct)
        {
            var request = await _context.Request
                .SingleOrDefaultAsync(r => r.Id == requestId, ct);
            if (request == null) return;

            // TODO: Will need to delete records in other tables first then delete the request

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
