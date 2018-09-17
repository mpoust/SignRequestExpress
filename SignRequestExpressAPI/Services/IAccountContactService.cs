////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: IAccountContactService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified:
 * Description: This service wraps AccountContact data access to keep the AccountContactController as thin as possible. This way we 
 *  rely on a service to  interact with the DB context and separate the data access concerns from the controller. 
 *  
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using SignRequestExpressAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Services
{
    public interface IAccountContactService
    {
        Task<AccountContact> GetAccountContactAsync(
            int id,
            CancellationToken ct);

        Task<IEnumerable<AccountContact>> GetAccountContactsAsync(
            CancellationToken ct);
    }
}
