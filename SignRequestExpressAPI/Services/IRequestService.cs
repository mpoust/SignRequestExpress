////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: IRequestService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 9/21/2018
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


        Task<Guid> CreateRequestAsync(
            Guid userId,
            string reason,
            byte status,
            //DateTime requestedDate,
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
            );

        Task DeleteRequestAsync(Guid requestId, CancellationToken ct);
    }
}
