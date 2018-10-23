using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignRequestExpress.Models.AccountViewModels;
using SignRequestExpress.Models.PostModels;
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
    
        public IActionResult Index()
        {   
            // Do I get the data for all the partial views here?  Is there a better place to process this?
            //  do with a View Component instead of partialview?  Need to learn more

            // Get Accounts for user with an API call
            

            return View();
        }

        //
        // POST: /Sales (CreateRequestPartial)
        [HttpPost]
        public async Task<IActionResult> SubmitRequest()
        {
            // GET UserInfo - UserID to send with Request POST
            var apiToken = HttpContext.Session.GetString(SessionKeyName);

            // Get UserInfo
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            var request = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var info = response.Content.ReadAsStringAsync().Result;
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(info);
                var id = userInfo.Id;
            }

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
