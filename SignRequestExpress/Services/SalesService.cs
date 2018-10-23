using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignRequestExpress.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignRequestExpress.Services
{
    public class SalesService
    {
        public SalesService()
        {

        }

        public async Task<Guid> GetSalesId(HttpClient httpClient, string apiToken)
        {
            // Get UserInfo
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            var request = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            var response = await httpClient.SendAsync(request);
            var id = new Guid(); //TODO: Make error return proper

            if (response.IsSuccessStatusCode)
            {
                var info = response.Content.ReadAsStringAsync().Result;
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(info);
                id = userInfo.Id;
            }
            return id;
        }
    }
}
