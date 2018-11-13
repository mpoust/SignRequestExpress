using Hanssens.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignRequestExpress.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
