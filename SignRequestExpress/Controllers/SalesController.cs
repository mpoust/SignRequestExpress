using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignRequestExpress.Models.AccountViewModels;
using SignRequestExpress.Models.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public async Task<IActionResult> Index()
        {
            /*
            var request = new HttpRequestMessage(HttpMethod.Post, "/user");
            
            var postUser = JsonConvert.SerializeObject(new PostUser
            {
                FirstName = "test",
                LastName = "sales",
                Username = "testsales",
                Password = "Password123!",
                PhoneNumber = null,
                Email = "test@sales.com",
                Role = "Sales"
            });


            var response = _httpClient.PostAsync("/user",
                                    new StringContent(postUser,
                                                      Encoding.UTF8,
                                                      "application/json"));
                                                      */

           // ViewData["Test"] = response.Con

            return View();
        }
    }
}
