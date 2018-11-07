using Hanssens.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SignRequestExpress.Models.Azure;
using SignRequestExpress.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignRequestExpress.Controllers
{
    [Authorize(Policy = "SignShopPolicy")]
    public class SignShopController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly StorageAccountOptions _storageAccountOptions;
        private readonly BlobUtility _blobUtility;

        public const string ApiClient = "sreApi";

        public SignShopController(
            IHttpClientFactory clientFactory,
            IOptions<StorageAccountOptions> storageAccountOptions)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient(ApiClient);
            _storageAccountOptions = storageAccountOptions.Value;
            _blobUtility = new BlobUtility(
                _storageAccountOptions.StorageAccountNameOption,
                _storageAccountOptions.StorageAccountKeyOption);
        }


        public async Task<IActionResult> Index()
        {
            SetHeaderWithApiToken(_httpClient);

            return View();
        }

        // Use this to seed an ajax call when the request queue view is opened
        [HttpPost]
        public async Task<List<SignRequest>> GetQueueTemplates()
        {
            SetHeaderWithApiToken(_httpClient);

            // Get Requests where Status = 1 (Approved) or 2 (In Queue)
            var approvedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 1");
            var approvedResponse = await _httpClient.SendAsync(approvedRequest);

            var queueRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 2");
            var queueResponse = await _httpClient.SendAsync(queueRequest);

            if (approvedResponse.IsSuccessStatusCode && queueResponse.IsSuccessStatusCode)
            {
                var approvedInfo = approvedResponse.Content.ReadAsStringAsync().Result;
                var approvedJsonData = JsonConvert.DeserializeObject<CollectionResponse>(approvedInfo).Value;

                var queueInfo = queueResponse.Content.ReadAsStringAsync().Result;
                var queueJsonData = JsonConvert.DeserializeObject<CollectionResponse>(queueInfo).Value;

                //List<SignRequest> data = approvedJsonData.ToList<SignRequest>();
                List<SignRequest> data = new List<SignRequest>();

                //approvedJsonData.
                foreach (var approved in approvedJsonData)
                {
                    SignRequest request = new SignRequest(approved);
                    data.Add(request);

                }



                //return keystring;
                //eturn data.First().RequestNumber;
                //return approvedInfo;

                return data;
            }
            else
            {
                return null;
            }

        }

        // Helper Methods -- TODO: Fix other methods, make private static void
        private static void SetHeaderWithApiToken(HttpClient httpClient)
        {
            // Get Token from LocalStorage
            using (var storage = new LocalStorage())
            {
                // TODO: add check for if the token is expired, then get a refresh
                var apiToken = storage.Get("ApiTokenStorage");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken.ToString());
            }            
        }
    }
}
