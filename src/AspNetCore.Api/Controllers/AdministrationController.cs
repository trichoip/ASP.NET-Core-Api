using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        public IActionResult Index() => Content("Administrator");
    }

    [Authorize(Roles = "HRManager,Finance")]
    public class SalaryController : Controller
    {
        public IActionResult Payslip() => Content("HRManager || Finance");
    }

    [Authorize(Roles = "PowerUser")]
    [Authorize(Roles = "ControlPanelUser")]
    public class ControlPanelController : Controller
    {
        public IActionResult Index() => Content("PowerUser && ControlPanelUser");
    }

    [Authorize(Roles = "Administrator,PowerUser")]
    public class ControlAllPanelController : Controller
    {
        public IActionResult SetTime() => Content("Administrator || PowerUser");

        [Authorize(Roles = "Administrator")]
        public IActionResult ShutDown() => Content("Administrator only");
    }

    [Authorize]
    public class Control3PanelController : Controller
    {
        public IActionResult SetTime() => Content("[Authorize]");

        [AllowAnonymous]
        public IActionResult Login() => Content("[AllowAnonymous]");
    }

}
