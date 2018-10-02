using Microsoft.AspNetCore.Mvc;
using SignRequestExpress.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SignRequestExpress.Controllers
{
    public class SalesController : Controller
    {
        //
        // GET: /sales 
        public IActionResult Index() // Index is default method called if none are specified
        {
            return View();
        }

        /*
        // numTimes defaults to 1 if no parameter is provided. This method uses ModelBinding
        // GET: /sales
        public string Name(string name, int numTimes = 1)
        {
            // HtmlEncoder.Default.Encode protects the app from malicious input (namely JavaScript)
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }
        */

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }

        //
        // GET: /sales/contact
        public string Contact()
        {
            string message = "This is Sales Contact";
            return message;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
