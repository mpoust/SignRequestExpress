﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpress.Controllers
{
    [Authorize(Policy = "AdministratorPolicy")]
    public class AdministratorController : Controller
    {

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
