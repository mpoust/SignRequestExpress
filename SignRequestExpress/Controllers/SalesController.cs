using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpress.Controllers
{
    [Authorize(Policy = "SalesPolicy")]
    // [AllowAnonymous] will let unauthorized users access if there is an action within I need to allow
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
