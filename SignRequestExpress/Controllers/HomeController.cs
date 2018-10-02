using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SignRequestExpress.Models;

/*
 * Will probably end up turning this into the default landing page, then once you login have the organization specific 
 *  and role specific pages displayed
 * 
 */


namespace SignRequestExpress.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiSettings _apiSettings;

        public HomeController(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            CompanyInfo companyInfo;
            

            ViewData["Message"] = "Pulling contact information from API.";
            ViewData["ApiUrl"] = new Uri(_apiSettings.ApiUrl);

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiSettings.ApiUrl);
                client.DefaultRequestHeaders.Clear(); // Can add Bearer token here later?
                // Define request data format - our API uses application/ion+json
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/ion+json")); // Should I move this connection to someplace else where it applies everywhere?
                // Sending request to find web API REST service resource info
                HttpResponseMessage message = await client.GetAsync("/info");

                if (message.IsSuccessStatusCode)
                {
                    var infoResponse = message.Content.ReadAsStringAsync().Result;
                    companyInfo = JsonConvert.DeserializeObject<CompanyInfo>(infoResponse);
                    ViewData["Name"] = companyInfo.Name;
                }
                else
                {
                    ViewData["Name"] = "Something went wrong";
                }
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
