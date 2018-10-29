using Hanssens.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignRequestExpress.Models.AccountViewModels;
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

        public const string ApiClient = "sreApi";
        public const string SessionKeyName = "_APIToken";
        public const string UserInfoRoute = "/userinfo";

        public const string AdhesiveCase = "Adhesive";
        public const string PhotoGlossyCase = "Photo Glossy";
        public const string MatteOutdoorCase = "Matte Outdoor";
        public const string PlasticoreCase = "Plasticore";

        // TODO - add new cases for adhesive w/ and w/o corrplast, and plasticore options
        public const byte Adhesive = 1;
        public const byte PhotoGlossy = 2;
        public const byte MatteOutdoor = 3;
        public const byte Plasticore = 4;

        public SalesController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IHttpClientFactory clientFactory,
            ISalesService salesService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient(ApiClient);
            _salesService = salesService;
        }

        // DEVELOPMENT TESTING ONLY
        public async Task<IActionResult> TestPage()
        {
            //SalesService salesService = new SalesService(); // Turn the whole process into a method - seed accounts, seed brands, etc inside salesService

            var apiToken = HttpContext.Session.GetString(SessionKeyName); // TODO: factor this out into a service
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

            var request = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var info = response.Content.ReadAsStringAsync().Result;
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(info);
                var id = userInfo.Id;
                ViewData["test2"] = id;

                var request2 = new HttpRequestMessage(HttpMethod.Get, $"/accounts/sales/{id}");
                var response2 = await _httpClient.SendAsync(request2);

                if (response2.IsSuccessStatusCode)
                {
                    var info2 = response2.Content.ReadAsStringAsync().Result;
                    //List<SalesAccounts> accountsList = JsonConvert.DeserializeObject<List<SalesAccounts>>(info2);
                    // accountsList = JsonConvert.DeserializeObject<SalesAccounts[]>(info2);
                    //ViewBag.AccountsList = accountsList;

                    var data = JsonConvert.DeserializeObject<CollectionResponse>(info2).Value;

                    var AccountList = new List<object>();
                    foreach(var accounts in data)
                    {
                        ViewData["test3"] += "New Account\r\n"; // How to use new lines..?!

                        foreach (var kvp in accounts)
                        {

                            //string msg = "Key = " + kvp.Key + ", Value = " + kvp.Value + " | ";
                            string msg = "";
                          // var AccountList = new List<string>();
                            
                            if(kvp.Key == "accountName")
                            {
                                msg += kvp.Value + ", ";
                                AccountList.Add(kvp.Value);
                            }

                            ViewData["test4"] += msg;

                        }
                    }
                    ViewBag.AccountList = AccountList;

                }
            }
            return View();
        }
    
        public async Task<IActionResult> Index()
        {
            // Do I get the data for all the partial views here?  Is there a better place to process this?
            //  do with a View Component instead of partialview?  Need to learn more

            SetHeaderWithApiToken(_httpClient);
            //var userId = GetUserId(); // Doesn't seem to work?

            // TODO: Only go through requests if there is no data in LocalStorage for Accounts, Brands - Will need refresh if added.

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

                    var AccountList = new List<object>(); // List use to seed account selection dropdown

                    foreach (var account in userJsonData)
                    {
                        foreach (var kvp in account)
                        {
                            if (kvp.Key == "accountName")
                            {
                                AccountList.Add(kvp.Value);
                            }
                        }
                    }
                    // Alphabetize
                    AccountList.Sort();
                    // Create ViewBag for use in the PartialView
                    ViewBag.AccountList = AccountList;

                    /*
                    // Test of LocalStorage
                    using(var storage = new LocalStorage())
                    {
                        storage.Store("accountStorage", AccountList);
                        storage.Persist();
                    }
                    */
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
                    // Create ViewBag 
                    ViewBag.BrandList = BrandList;
                }
            }
            return View();
        }

        //
        // POST: /Sales (CreateRequestPartial)
        [HttpPost]
        public async Task<IActionResult> SubmitRequest(SignRequestModel model)
        {
            // GET UserInfo - UserID to send with Request POST
            SetHeaderWithApiToken(_httpClient);            

            if (ModelState.IsValid) // Model state is clearly not valid.  What is going wrong???
            {
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

                    userId = new Guid("0be59332-bd8f-484d-9f35-0dc17850d23b");
                    Guid templateId = new Guid("066f4396-e614-49d2-94f9-57873aafc29b");

                    var postRequest = JsonConvert.SerializeObject(new PostSignRequest
                    {
                        // NEED TO GET USER ID FUNCTION WORKING
                        
                        UserId = userId,
                        Reason = model.Reason,
                        NeededDate = model.NeededDate,     // TODO - add when datepicker implemented
                        IsProofNeeded = false, // TODO - connect to checkbox
                        MediaFK = mediaFk,
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
            //return PartialView("_CreateRequestPartial", model); // This result was interesting

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

        // Partial View Helper method
        [NonAction]
        public virtual PartialViewResult PartialView(string viewName, object model)
        {
            ViewData.Model = model;

            return new PartialViewResult()
            {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
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
