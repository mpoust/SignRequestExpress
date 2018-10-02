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
 * why is continous deployment so weird
 */


namespace SignRequestExpress.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory; 

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var client = _clientFactory.CreateClient("sreApi");

            ViewData["Message"] = "Pulling contact information from API. But is it working??";
            ViewData["ApiUrl"] = new Uri(client.BaseAddress.ToString());

            HttpResponseMessage message = await client.GetAsync("/info");
            if (message.IsSuccessStatusCode)
            {
                var infoResponse = message.Content.ReadAsStringAsync().Result;
                CompanyInfo companyInfo = JsonConvert.DeserializeObject<CompanyInfo>(infoResponse);
                ViewData["Name"] = companyInfo.Name;
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
