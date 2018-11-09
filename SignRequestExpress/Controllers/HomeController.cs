using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignRequestExpress.Models;

namespace SignRequestExpress.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            //ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            // Compose Email
            //using(var message = new MailMessage(model.Email, "michael.poust221@gmail.com"))
            //{
            //    message.To.Add(new MailAddress("michael.poust221@gmail.com"));
            //    message.From = new MailAddress(model.Email);
            //    message.Subject = model.Subject;
            //    message.Body = model.Message;

            //    using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            //    {
            //        smtpClient.Send(message);
            //    }
            //}

           
            // If we got this far something went wrong, redisplay form
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Help()
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
