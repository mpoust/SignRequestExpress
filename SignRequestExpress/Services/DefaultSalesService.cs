////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: SalesService.cs
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignRequestExpress.Models.PostModels;
using SignRequestExpress.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignRequestExpress.Services
{
    public class DefaultSalesService : ISalesService
    {
        //private readonly HttpClient _httpClient;

        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        private const string UserInfoRoute = "/userinfo";
        public const string SessionKeyName = "_APIToken";

        public DefaultSalesService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("sreApi");
        }

        // TODO: as request is posted
        public Task<(bool Suceeded, string ErrorMessage)> CreateRequestAsync(PostSignRequest postSignRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> GetSalesId(HttpClient httpClient, string apiToken)
        {
            Guid userId;

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, UserInfoRoute);
            var response = await httpClient.SendAsync(requestMsg);

            if (response.IsSuccessStatusCode)
            {
                var info = response.Content.ReadAsStringAsync().Result;
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(info);
                userId = userInfo.Id;
                return userId;
            }
            else
            {
                // TODO: fix error
                throw new NotImplementedException();
            }
            

            
        }
    }
}
