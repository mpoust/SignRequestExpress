using Hanssens.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignRequestExpress.Models.Azure;
using SignRequestExpress.Models.SignShopModels;
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
        public async Task<List<SignRequest>> GetQueueTemplates()
        {
            throw new NotImplementedException();
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
