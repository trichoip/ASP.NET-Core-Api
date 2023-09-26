using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_core_empty_5._0.Controllers
{
    public class FeatureController : Controller
    {
        public IActionResult Index1()
        {

            return View();
        }

        public IActionResult List()
        {

            return View();
        }

    }
}