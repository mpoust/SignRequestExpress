////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: ISalesService.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/25/2018
 * Last Modified: 
 * Description: 
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using SignRequestExpress.Models.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SignRequestExpress.Services
{
    public interface ISalesService
    {
        Task<(bool Suceeded, string ErrorMessage)> CreateRequestAsync(PostSignRequest postSignRequest);

        Task<Guid> GetSalesId(HttpClient httpClient, string apiToken);
    }
}
