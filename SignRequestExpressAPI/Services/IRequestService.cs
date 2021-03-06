﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: IRequestService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 11/15/2018
 * Description: This service wraps Request data access to keep the RequestController as thin as possible. This way we rely on a service to 
 *  interact with the DB context and separate the data access concerns from the controller. This has the added benefit of making the controller
 *  easier to test because we can inject a mock service instead of having to mock the entire database context. 
 *  
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Services
{
    public interface IRequestService
    {
        Task<Request> GetRequestAsync(
            Guid id,
            CancellationToken ct);

        Task<PagedResults<Request>> GetRequestsAsync(
            PagingOptions pagingOptions,
            SortOptions<Request, RequestEntity> sortOptions,
            SearchOptions<Request, RequestEntity> searchOptions,
            CancellationToken ct);

        Task<PagedResults<Request>> GetUserRequestsAsync(
            Guid? userId,
            PagingOptions pagingOptions,
            SortOptions<Request, RequestEntity> sortOptions,
            SearchOptions<Request, RequestEntity> searchOptions,
            CancellationToken ct);

        Task<Guid> CreateRequestAsync(
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
            );

        Task DeleteRequestAsync(Guid requestId, CancellationToken ct);

        Task<Guid?> GetTemplateIdAsync(string Uri, CancellationToken ct);

        Task UpdateRequestAsync(Request request, Guid id, CancellationToken ct);
    }
}
