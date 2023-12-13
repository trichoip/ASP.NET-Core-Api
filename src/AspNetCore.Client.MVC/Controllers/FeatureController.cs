using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Client.MVC.Controllers;

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