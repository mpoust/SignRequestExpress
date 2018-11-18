using Hanssens.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    [Authorize(Policy = "AdministratorPolicy")]
    public class AdministratorController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public const string ApiClient = "sreApi";

        public AdministratorController(
            IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient(ApiClient);
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetRequestsForApproval()
        {
            SetHeaderWithApiToken(_httpClient);

            // Get Requests where Status = 0 (Submitted waiting for approval)
            var submittedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 0");
            var submittedResponse = await _httpClient.SendAsync(submittedRequest);

            if (submittedResponse.IsSuccessStatusCode)
            {
                var submittedInfo = submittedResponse.Content.ReadAsStringAsync().Result;
                var submittedJsonData = JsonConvert.DeserializeObject<CollectionResponse>(submittedInfo).Value;

                List<SignRequest> data = new List<SignRequest>();

                foreach(var submitted in submittedJsonData)
                {
                    SignRequest request = new SignRequest(submitted);
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

            var approvedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 1");
            var approvedResponse = await _httpClient.SendAsync(approvedRequest);

            var queueRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 2");
            var queueResponse = await _httpClient.SendAsync(queueRequest);

            var printedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 3");
            var printedResponse = await _httpClient.SendAsync(printedRequest);

            List<SignRequest> data = new List<SignRequest>();

            if (approvedResponse.IsSuccessStatusCode)
            {
                var approvedInfo = approvedResponse.Content.ReadAsStringAsync().Result;
                var submittedJsonData = JsonConvert.DeserializeObject<CollectionResponse>(approvedInfo).Value;                

                foreach (var approved in submittedJsonData)
                {
                    SignRequest request = new SignRequest(approved);
                    data.Add(request);
                }                
            }

            if (queueResponse.IsSuccessStatusCode)
            {
                var queueInfo = queueResponse.Content.ReadAsStringAsync().Result;
                var queueJsonData = JsonConvert.DeserializeObject<CollectionResponse>(queueInfo).Value;

                foreach (var queue in queueJsonData)
                {
                    SignRequest request = new SignRequest(queue);
                    data.Add(request);
                }
            }

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

        [HttpPost]
        public async Task<IActionResult> ApproveRequest([FromBody] string requestId)
        {
            SetHeaderWithApiToken(_httpClient);

            var patchRequest = JsonConvert.SerializeObject(new StatusPatch
            {
                Op = "replace",
                Path = "/status",
                Value = "1" // This means Approved
            });

            var testPatch = "[{\"op\":\"replace\", \"path\":\"/status\",\"value\":1}]"; // This approves

            var uri = "https://signrequestexpressapi.azurewebsites.net/requests/" + requestId.ToLower() + "/";

            var patchResponse = await _httpClient.PatchAsync(uri,
                                    new StringContent(testPatch, Encoding.UTF8, "application/json"));

            if (patchResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AdministratorController.Index), "Administrator");
            }
            else
            {
                throw new Exception();
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
