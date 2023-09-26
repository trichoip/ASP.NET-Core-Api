using System.Collections.Generic;
using System.Linq;
using asp.net_core_empty_5._0.Models;
using asp.net_core_empty_5._0.Repositories.ServiceBase;
using asp.net_core_empty_5._0.Repositories.ServiceBase.Impl;
using asp.net_core_empty_5._0.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace asp.net_core_empty_5._0.Controllers
{
    public class SessionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AccountService _accountService;
        public SessionController(IConfiguration configuration)
        {
            _configuration = configuration;
            _accountService = new AccountServiceImpl();
        }
        //public SessionController(IConfiguration configuration, AccountService accountService)
        //{
        //    _configuration = configuration;
        //    _accountService = accountService;
        //}

        //[ViewData]
        [TempData]
        public string Messages { get; set; }
        [BindProperty]
        public AccountUserVM AccountUserVM { get; set; }
        public IActionResult Session()
        {

            //  services.AddSession();
            //  app.UseSession();

            // razor page client size 
            // @* @HttpContext.Session.GetString("Email") *@

            // mvc client size
            // @* @Context.Session.GetString("Email") *@

            // razor page server size and mvc server size
            HttpContext.Session.SetString("Test", "Hello World");
            HttpContext.Session.SetInt32("TestInt", 123);
            HttpContext.Session.Set("TestObj", new byte[] { 0x0, 0x1, 0x2, 0x3 });

            HttpContext.Session.Get("key");
            HttpContext.Session.GetString("Test");
            HttpContext.Session.GetInt32("TestInt");

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("key");

            var accountUsers2 = _configuration.GetSection("AccountUser").Get<List<Dictionary<string, string>>>();
            var user2 = accountUsers2.FirstOrDefault(u => u["Email"] == AccountUserVM.Email && u["Password"] == AccountUserVM.Password);

            var superAccount2 = _configuration.GetSection("SuperAccount").Get<Dictionary<string, string>>();
            bool checkSuperAdmin2 = superAccount2 != null && superAccount2["Email"] == AccountUserVM.Email && superAccount2["Password"] == AccountUserVM.Password;

            var superEmail = _configuration.GetValue<string>("SuperAccount:Email");
            var superPassword = _configuration.GetValue<string>("SuperAccount:Password");
            var superRole = _configuration.GetValue<string>("SuperAccount:Role");

            return View();
        }
        public IActionResult Login()
        {
            List<Account> accounts = _accountService.GetAll();

            TempData["Message"] = "Hello world!";
            //TempData.Peek("Message");
            //TempData.Keep("Message");
            Messages = "Hello world!";

            if (HttpContext.Request.Method == "GET")
            {
                return RedirectToAction("index");
            }

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var accountUsers = _configuration.GetSection("AccountUser").Get<List<AccountUserVM>>();
            var superAccount = _configuration.GetSection("SuperAccount").Get<AccountUserVM>();

            var user = accountUsers.FirstOrDefault(u => u.Email == AccountUserVM.Email && u.Password == AccountUserVM.Password);
            bool checkSuperAdmin = superAccount.Email == AccountUserVM.Email && superAccount.Password == AccountUserVM.Password;

            if (user != null)
            {
                if (user.Role == "Admin")
                {
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("Role", user.Role);
                    return RedirectToAction("Admin");
                }
                else if (user.Role == "User")
                {
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("Role", user.Role);
                    return RedirectToAction("Users");
                }

            }
            else if (checkSuperAdmin)
            {
                HttpContext.Session.SetString("Email", superAccount.Email);
                HttpContext.Session.SetString("Role", superAccount.Role);
                return RedirectToAction("SuperAdmin");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }

            return View("Index");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Admin()
        {
            var Role = HttpContext.Session.GetString("Role");

            if (Role == null || Role != "Admin")
            {
                ModelState.AddModelError(string.Empty, "you need role Admin access admin page");
                return View("Index");
            }

            return View();
        }

        public IActionResult Users()
        {
            var Role = HttpContext.Session.GetString("Role");

            if (Role == null || Role != "User")
            {
                ModelState.AddModelError(string.Empty, "you need role User access User page");
                return View("Index");
            }
            return View();
        }

        public IActionResult SuperAdmin()
        {
            var Role = HttpContext.Session.GetString("Role");

            if (Role == null || Role != "Super")
            {
                ModelState.AddModelError(string.Empty, "you need role Super access Super page");
                return View("Index");
            }
            return View();
        }
    }
}
