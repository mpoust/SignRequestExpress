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

        public const string SessionKeyName = "_APIToken";

        public SalesController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IHttpClientFactory clientFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("sreApi");
        }

        // DEVELOPMENT TESTING ONLY
        public async Task<IActionResult> TestPage()
        {
            SalesService salesService = new SalesService();

            var apiToken = HttpContext.Session.GetString(SessionKeyName); // TODO: factor this out into a service
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            // var id = salesService.GetSalesId(_httpClient, apiToken); //this does not work

            var request = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            var response = await _httpClient.SendAsync(request);

           // ViewData["test1"] = apiToken.ToString();

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

                    var data = JsonConvert.DeserializeObject<SalesAccountsResponse>(info2).Value;

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
            // Get Accounts for user with an API call
            var apiToken = HttpContext.Session.GetString(SessionKeyName); // TODO: factor this out into a service
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

            // Getting userID from /userinfo to pull Accounts tied to that user.
            var userinfoRequest = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            var userinfoResponse = await _httpClient.SendAsync(userinfoRequest);

            if (userinfoResponse.IsSuccessStatusCode)
            {
                var userinfo = userinfoResponse.Content.ReadAsStringAsync().Result;
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userinfo);
                var userId = userInfo.Id;

                // Have ID, get accounts
                var accountRequest = new HttpRequestMessage(HttpMethod.Get, $"/accounts/sales/{userId}");
                var accountResponse = await _httpClient.SendAsync(accountRequest);

                if (accountResponse.IsSuccessStatusCode)
                {
                    var accountInfo = accountResponse.Content.ReadAsStringAsync().Result;
                    var jsonData = JsonConvert.DeserializeObject<SalesAccountsResponse>(accountInfo).Value; // is there a way to deserialize into my model?

                    var AccountList = new List<object>(); // List use to seed account selection dropdown

                    foreach (var account in jsonData)
                    {
                        foreach (var kvp in account)
                        {
                            if (kvp.Key == "accountName")
                            {
                                AccountList.Add(kvp.Value);
                            }
                        }
                    }
                    // Sort the List
                    AccountList.Sort();

                    // Create ViewBag for use in the PartialView
                    ViewBag.AccountList = AccountList;
                }
            }
            return View();
        }


        //
        // POST: /Sales (CreateRequestPartial)
        [HttpPost]
        public async Task<IActionResult> SubmitRequest()
        {
            SalesService salesService = new SalesService();

            // GET UserInfo - UserID to send with Request POST
            var apiToken = HttpContext.Session.GetString(SessionKeyName);

            // Get Sales User ID - for POSTing the Request
            var id = salesService.GetSalesId(_httpClient, apiToken);

            // TODO: Post the Request to the API

            // Return to success page if request submitted successfully
            return RedirectToAction(nameof(SalesController.RequestSubmitted), "Sales");
        }

        public async Task<IActionResult> RequestSubmitted()
        {
            // TODO: Make so redirect only can be viewed from submit button
            //      add capability to show request details like a receipt details
            return View();
        }
    }
}
