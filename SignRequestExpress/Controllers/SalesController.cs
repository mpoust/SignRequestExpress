// COMMENT

using Hanssens.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SignRequestExpress.Models.AccountViewModels;
using SignRequestExpress.Models.Azure;
using SignRequestExpress.Models.PostModels;
using SignRequestExpress.Models.ResponseModels;
using SignRequestExpress.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SignRequestExpress.Controllers
{
    [Authorize(Policy = "SalesPolicy")]
    // [AllowAnonymous] will let unauthorized users access if there is an action within that I need to allow
    public class SalesController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly ISalesService _salesService;
        private readonly StorageAccountOptions _storageAccountOptions;
        private readonly BlobUtility _blobUtility;

        public const string ApiClient = "sreApi";
        public const string SessionKeyName = "_APIToken";
        public const string UserInfoRoute = "/userinfo";

        public const string AdhesiveCase = "Adhesive";
        public const string PhotoGlossyCase = "Photo Glossy";
        public const string MatteOutdoorCase = "Matte Outdoor";
        public const string PlasticoreCase = "Plasticore";

        // TODO - add new cases for adhesive w/ and w/o corrplast, and plasticore options ---- Could have made option value property... 
        // I made this more difficult than necessary
        // no longer used - make sure still works before delete
        public const byte Adhesive = 1;
        public const byte PhotoGlossy = 2;
        public const byte MatteOutdoor = 3;
        public const byte Plasticore = 4;

        public SalesController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IHttpClientFactory clientFactory,
            ISalesService salesService,
            IOptions<StorageAccountOptions> storageAccountOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient(ApiClient);
            _salesService = salesService;
            _storageAccountOptions = storageAccountOptions.Value;
            _blobUtility = new BlobUtility(
                _storageAccountOptions.StorageAccountNameOption,
                _storageAccountOptions.StorageAccountKeyOption);
        }

        public async Task<IActionResult> Index()
        {
            // Do I get the data for all the partial views here?  Is there a better place to process this?
            //  do with a View Component instead of partialview?  Need to learn more

            SetHeaderWithApiToken(_httpClient);
            //var userId = GetUserId(); // Doesn't seem to work?

            // TODO: Only go through requests below if there is no data in LocalStorage for Accounts, Brands - Will need refresh if added.

            // Getting userID from /userinfo to pull Accounts tied to that user.
            var userinfoRequest = new HttpRequestMessage(HttpMethod.Get, UserInfoRoute);
            var userinfoResponse = await _httpClient.SendAsync(userinfoRequest);
            
            if (userinfoResponse.IsSuccessStatusCode)
            {
                var userinfo = userinfoResponse.Content.ReadAsStringAsync().Result;
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userinfo);
                var userId = userInfo.Id;
                
                // Get Accounts for user with an API call
                // Create Account List
                var accountRequest = new HttpRequestMessage(HttpMethod.Get, $"/accounts/sales/{userId}");
                var accountResponse = await _httpClient.SendAsync(accountRequest);

                if (accountResponse.IsSuccessStatusCode)
                {                                   
                    var accountInfo = accountResponse.Content.ReadAsStringAsync().Result;
                    var userJsonData = JsonConvert.DeserializeObject<CollectionResponse>(accountInfo).Value; // is there a way to deserialize into my model?

                    var AccountList = new List<string>(); // List use to seed account selection dropdown

                    foreach (var account in userJsonData)
                    {
                        foreach (var kvp in account)
                        {
                            if (kvp.Key == "accountName")
                            {
                                AccountList.Add(kvp.Value.ToString());
                            }
                        }
                    }
                    // Alphabetize
                    AccountList.Sort();

                    // Create accountStorage - data will now persist through failed submits
                    using(var storage = new LocalStorage())
                    {
                        storage.Store("accountStorage", AccountList);
                        storage.Persist();
                    }
                    
                }

                // Create Brand List - Potentially cache or store this stuff locally and check for new on login? -- Same with Accounts
                var brandListRequest = new HttpRequestMessage(HttpMethod.Get, "/brands");
                var brandinfoResponse = await _httpClient.SendAsync(brandListRequest);

                if (brandinfoResponse.IsSuccessStatusCode)
                {
                    var brandInfo = brandinfoResponse.Content.ReadAsStringAsync().Result;
                    var brandJsonData = JsonConvert.DeserializeObject<CollectionResponse>(brandInfo).Value;

                    var BrandList = new List<object>();

                    foreach (var brand in brandJsonData)
                    {
                        foreach (var kvp2 in brand)
                        {
                            if (kvp2.Key == "brandName")
                            {
                                BrandList.Add(kvp2.Value);
                            }
                        }
                    }
                    // Alphabetize
                    BrandList.Sort();
                    ViewBag.BrandList = BrandList;
                    // Create brandStorage
                    using (var storage = new LocalStorage())
                    {
                        storage.Store("brandStorage", BrandList);
                        storage.Persist();
                    }
                }
            }
            return View();
        }

        //
        // POST: /Sales (CreateRequestPartial)
        [HttpPost]
        public async Task<IActionResult> SubmitRequest(SignRequestModel model)
        {
            SetHeaderWithApiToken(_httpClient);            

            if (ModelState.IsValid)
            {
                /*
                byte mediaFk;

                switch (model.MediaString)
                {
                    case AdhesiveCase:
                        mediaFk = Adhesive;
                        break;
                    case PhotoGlossyCase:
                        mediaFk = PhotoGlossy;
                        break;
                    case MatteOutdoorCase:
                        mediaFk = MatteOutdoor;
                        break;
                    case PlasticoreCase:
                        mediaFk = Plasticore;
                        break;
                    default:
                        mediaFk = Adhesive;
                        break;
                }
                */

                bool isVertical;
                if (model.HeightInch > model.WidthInch) isVertical = true;
                else isVertical = false;
                
                // Get userID - TODO: REPLACE WITH A METHOD THAT WORKS
                var userinfoRequest = new HttpRequestMessage(HttpMethod.Get, UserInfoRoute);
                var userinfoResponse = await _httpClient.SendAsync(userinfoRequest);

                if (userinfoResponse.IsSuccessStatusCode)
                {
                    var userinfo = userinfoResponse.Content.ReadAsStringAsync().Result;
                    UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userinfo);
                    Guid userId = userInfo.Id;

                    var postRequest = JsonConvert.SerializeObject(new PostSignRequest
                    {
                        // NEED TO GET USER ID FUNCTION WORKING
                        
                        UserId = userId,
                        Reason = model.Reason,
                        NeededDate = Convert.ToDateTime(model.NeededDate),     // TODO - add when datepicker implemented
                        IsProofNeeded = false, // TODO - connect to checkbox
                        MediaFK = model.MediaFK,
                        Quantity = model.Quantity,
                        IsVertical = isVertical,
                        HeightInch = model.HeightInch,
                        WidthInch = model.WidthInch,
                        Template = model.Template,    // TODO - add when template selected implemented
                        Information = model.Information,
                        DataFileUri = model.DataFileUri,
                        ImageUri = model.ImageUri 
                    });

                    var responseRequest = await _httpClient.PostAsync("https://signrequestexpressapi.azurewebsites.net/requests",
                                            new StringContent(postRequest, Encoding.UTF8, "application/json"));

                    if (responseRequest.IsSuccessStatusCode)
                    {
                        // Return to success page if request submitted successfully
                        return RedirectToAction(nameof(SalesController.RequestSubmitted), "Sales");
                    }
                }
                
            }
            // NOTE: When submitting the request, a POST needs to also occur to the
            //          Request_Account, User_Request

            // If we got this far, something failed, redisplay form            
            return View("Index", model); // If there is an error the Account and Brand dropdowns are not filled
        }

        // Used when a brand is selected to populate the template modal
        [HttpPost]
        //[Route("/Sales/GetTemplates")]
        // Changing return type from Task<List<IListBlobItem>> to Task<List<string>>
        public async Task<List<string>> GetTemplates([FromBody] Brand brand)
        {       
            List<IListBlobItem> templates = new List<IListBlobItem>();
            templates = await _blobUtility.GetTemplateBlobsByBrand(brand.BrandName);

            // Return the URI of each to use to display the templates - change return type to list of string?
            List<string> UriList = new List<string>();
            foreach (IListBlobItem item in templates)
            {
                UriList.Add(item.Uri.ToString());
            }

            return UriList;
        }

        [HttpPost]
        public string TestPost([FromBody] Brand brand)
        {
            string response = "Brand is " + brand.BrandName + "!";

            return response;
        }

        public IActionResult RequestSubmitted()
        {
            // TODO: Make so redirect only can be viewed from submit button
            //      add capability to show request details like a receipt details
            return View();
        }

        public IActionResult RequestSubmitError()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOldRequests()
        {
            SetHeaderWithApiToken(_httpClient);

            var finishedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 3");
            var finishedResponse = await _httpClient.SendAsync(finishedRequest);

            List<SignRequest> data = new List<SignRequest>();

            if (finishedResponse.IsSuccessStatusCode)
            {
                var finishedInfo = finishedResponse.Content.ReadAsStringAsync().Result;
                var finishedJsonData = JsonConvert.DeserializeObject<CollectionResponse>(finishedInfo).Value;

                foreach(var finished in finishedJsonData)
                {
                    SignRequest request = new SignRequest(finished);
                    data.Add(request);
                }
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetRequestStatus()
        {
            SetHeaderWithApiToken(_httpClient);

            // TODO: write method where sales gets their requests except those completed
            //var salesRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests");
            //var salesResponse = await _httpClient.SendAsync(salesRequest);

            var submittedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 0");
            var submittedResponse = await _httpClient.SendAsync(submittedRequest);

            var approvedRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 1");
            var approvedResponse = await _httpClient.SendAsync(approvedRequest);

            var queueRequest = new HttpRequestMessage(HttpMethod.Get, $"/requests?search=status eq 2");
            var queueResponse = await _httpClient.SendAsync(queueRequest);

            List<SignRequest> data = new List<SignRequest>();

            if (submittedResponse.IsSuccessStatusCode)
            {
                var submittedInfo = submittedResponse.Content.ReadAsStringAsync().Result;
                var submittedJsonData = JsonConvert.DeserializeObject<CollectionResponse>(submittedInfo).Value;

                foreach (var submitted in submittedJsonData)
                {
                    SignRequest request = new SignRequest(submitted);
                    data.Add(request);
                }
            }

            if (approvedResponse.IsSuccessStatusCode)
            {
                var approvedInfo = approvedResponse.Content.ReadAsStringAsync().Result;
                var approvedJsonData = JsonConvert.DeserializeObject<CollectionResponse>(approvedInfo).Value;

                foreach (var approved in approvedJsonData)
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

            return Json(data);
                  
        }

        
        // Helper Methods -- TODO: Fix other methods, make private static void
        private static void SetHeaderWithApiToken(HttpClient httpClient)
        {
            // Get Token from LocalStorage
            using(var storage = new LocalStorage())
            {
                // TODO: add check for if the token is expired, then get a refresh
                var apiToken = storage.Get("ApiTokenStorage");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken.ToString());
            }

            //var apiToken = HttpContext.Session.GetString(SessionKeyName);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
        }


        /*
        // used in posting a request and getting user accounts -- this one doesnt work..?
        public async Task<Guid> GetUserId()
        {
            SetHeaderWithApiToken();

            var userinfoRequest = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            var userinfoResponse = await _httpClient.SendAsync(userinfoRequest);

            if (userinfoResponse.IsSuccessStatusCode)
            {
                var userinfo = userinfoResponse.Content.ReadAsStringAsync().Result;
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userinfo);
                var userId = userInfo.Id;
                return userId;
            }
            else
            {
                throw new NotImplementedException(); // TODO fix errors
            }            
        }
        */
    }
}
