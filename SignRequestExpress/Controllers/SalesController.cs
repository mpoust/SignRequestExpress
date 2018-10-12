using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignRequestExpress.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SignRequestExpress.Controllers
{
    [Authorize(Policy = "SalesPolicy")]
    // [AllowAnonymous] will let unauthorized users access if there is an action within I need to allow
    public class SalesController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

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

        public async Task<IActionResult> Index()
        {
            // var client = _clientFactory.CreateClient("sreApi")
            //ViewData["apiUrl"] = new Uri(_httpClient.BaseAddress.ToString());

            // ViewData["apiUrl"] = test.ToString();


            // How the hell do I get data to create the token?

            // This will return the token appropriately. Now how do I store the data and use?  Bookmarked page to add item in authorization header.
            var request = new HttpRequestMessage(HttpMethod.Post, "/token");

            var tokenData = new List<KeyValuePair<string, string>>();
            tokenData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            tokenData.Add(new KeyValuePair<string, string>("username", "testuser")); // hardcoding to test
            tokenData.Add(new KeyValuePair<string, string>("password", "Password123!"));

            request.Content = new FormUrlEncodedContent(tokenData);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var info = response.Content.ReadAsStringAsync().Result;
                ViewData["apiUrl"] = info.ToString();
            }

            // Testing post for retrieving a token
           // var user = _signInManager.
            //var username = await _userManager.GetUserNameAsync();
            
            return View();
        }
    }
}
