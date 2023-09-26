using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.MVC.Controllers
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