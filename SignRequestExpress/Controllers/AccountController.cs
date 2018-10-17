using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SignRequestExpress.Models.AccountViewModels;
using SignRequestExpress.Models.PostModels;
using SignRequestExpress.Models.ResponseModels;

namespace SignRequestExpress.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public const string SessionKeyName = "_APIToken";
        public const string GrantType = "grant_type";
        public const string GrantTypeValue = "password";
        public const string UsernameKey = "username";
        public const string PasswordKey = "password";
        public const string ApiClient = "sreApi";

        private const string ExecutiveCase =     "498-01-RK01";
        private const string AdministratorCase = "498-01-RK02";
        private const string SalesCase =         "498-01-RK03";
        private const string SignShopCase =      "498-01-RK04";

        private const string ExecutiveRole =     "Executive";
        private const string AdministratorRole = "Administrator";
        private const string SalesRole =         "Sales";
        private const string SignShopRole =      "SignShop";
        private const string DefaultRole =       "Default";

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpClientFactory clientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient(ApiClient);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser()
                {
                    Email = model.Email,
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password); // SPA-DB

                string role;
                // Determine Role from Access Code - No Code put in Default role
                switch (model.AccessCode)
                {
                    case ExecutiveCase:
                        role = ExecutiveRole;
                        break;
                    case AdministratorCase:
                        role = AdministratorRole;
                        break;
                    case SignShopCase:
                        role = SignShopRole;
                        break;
                    case SalesCase:
                        role = SalesRole;
                        break;
                    default:
                        role = DefaultRole;
                        break;
                }

                // Assign user to role - SPA-DB
                await _userManager.AddToRoleAsync(user, role);

                if (result.Succeeded)
                {
                    // Post to API
                    var postUser = JsonConvert.SerializeObject( new PostUser
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Username = model.Username,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        Role = role
                    });

                    var response = await _httpClient.PostAsync("https://signrequestexpressapi.azurewebsites.net/users",
                                            new StringContent(postUser, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // Returns the view
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Does the action
        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {                        
                        // Get API Token with valid credentials
                        var request = new HttpRequestMessage(HttpMethod.Post, "/token");

                        var postData = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>(GrantType, GrantTypeValue),
                            new KeyValuePair<string, string>(UsernameKey, model.Username),
                            new KeyValuePair<string, string>(PasswordKey, model.Password)
                        };

                        request.Content = new FormUrlEncodedContent(postData);
                        var response = await _httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var info = response.Content.ReadAsStringAsync().Result;
                            ApiToken apiToken = JsonConvert.DeserializeObject<ApiToken>(info);
                            var token = apiToken.Access_Token;

                            // Create Session with API Access Token
                            HttpContext.Session.Clear(); // Clear any persisting session data
                            HttpContext.Session.SetString(SessionKeyName, token);
                        }

                        var user = await _userManager.FindByNameAsync(model.Username);

                        if (await _userManager.IsInRoleAsync(user, "Sales"))
                        {
                            return RedirectToAction(nameof(SalesController.Index), "Sales");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Executive"))
                        {
                            return RedirectToAction(nameof(ExecutiveController.Index), "Executive");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Administrator"))
                        {
                            return RedirectToAction(nameof(AdministratorController.Index), "Administrator");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "SignShop"))
                        {
                            return RedirectToAction(nameof(SignShopController.Index), "SignShop");
                        }
                        else
                        {
                            return RedirectToAction(nameof(DefaultController.Index), "Default");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); // Remove Session data on logout
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
