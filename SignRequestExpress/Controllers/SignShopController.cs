using Hanssens.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SignRequestExpress.Models.Azure;
using SignRequestExpress.Models.PatchModels;
using SignRequestExpress.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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


        public IActionResult Index()
        {
            SetHeaderWithApiToken(_httpClient);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueueRequest([FromBody] string requestId)
        {
            SetHeaderWithApiToken(_httpClient);

            var patchRequest = JsonConvert.SerializeObject(new StatusPatch
            {
                Op = "replace",
                Path = "/status",
                Value = "2" // In Queue
            });

            var testPatch = "[{\"op\":\"replace\", \"path\":\"/status\",\"value\":2}]"; // This queues

            var uri = "https://signrequestexpressapi.azurewebsites.net/requests/" + requestId.ToLower() + "/";

            var patchResponse = await _httpClient.PatchAsync(uri,
                                    new StringContent(testPatch, Encoding.UTF8, "application/json"));

            if (patchResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SignShopController.Index), "SignShop");
            }
            else
            {
                throw new Exception();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PrintRequest([FromBody] string requestId)
        {
            SetHeaderWithApiToken(_httpClient);

            var patchRequest = JsonConvert.SerializeObject(new StatusPatch
            {
                Op = "replace",
                Path = "/status",
                Value = "3" // Printed
            });

            var testPatch = "[{\"op\":\"replace\", \"path\":\"/status\",\"value\":3}]"; // This prints

            var uri = "https://signrequestexpressapi.azurewebsites.net/requests/" + requestId.ToLower() + "/";

            var patchResponse = await _httpClient.PatchAsync(uri,
                                    new StringContent(testPatch, Encoding.UTF8, "application/json"));

            if (patchResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SignShopController.Index), "SignShop");
            }
            else
            {
                throw new Exception();
            }
        }

        // Use this to seed an ajax call when the request queue view is opened
        [HttpPost]
        public async Task<IActionResult> GetQueueRequests()
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

                foreach (var approved in approvedJsonData)
                {
                    SignRequest request = new SignRequest(approved);
                    data.Add(request);
                }

                foreach(var queue in queueJsonData)
                {
                    SignRequest request = new SignRequest(queue);
                    data.Add(request);
                }

                return Json(data);
            }
            else
            {
                return null;
            }

        }

        public async Task<IActionResult> GetArchiveRequests()
        {
            SetHeaderWithApiToken(_httpClient);

            var printedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 3");
            var printedResponse = await _httpClient.SendAsync(printedRequest);

            List<SignRequest> data = new List<SignRequest>();

            if (printedResponse.IsSuccessStatusCode)
            {
                var printedInfo = printedResponse.Content.ReadAsStringAsync().Result;
                var printedJsonData = JsonConvert.DeserializeObject<CollectionResponse>(printedInfo).Value;

                foreach (var printed in printedJsonData)
                {
                    SignRequest request = new SignRequest(printed);
                    data.Add(request);
                }
            }

            return Json(data);
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
