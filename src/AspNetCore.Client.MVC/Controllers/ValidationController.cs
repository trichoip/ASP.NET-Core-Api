using AspNetCore.Client.MVC.Data;
using AspNetCore.Client.MVC.Models;
using AspNetCore.Client.MVC.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AspNetCore.Client.MVC.Controllers;

public class ValidationController : Controller
{
    private readonly ETransportationSystemContext _eTransportationSystemContext;
    public ValidationController(ETransportationSystemContext eTransportationSystemContext)
    {

        _eTransportationSystemContext = eTransportationSystemContext;

    }
    [BindProperty(SupportsGet = true)]
    public Account Account { get; set; }

    [BindProperty]
    public Movie Movie { get; set; }
    public IActionResult Index([BindRequired, MinLength(5)] string phone)
    {
        // ModelState check tất cả model binding trong class và  param trong function
        // nếu có 1 model binding nào đó mà khác null thì ModelState kích hoạt
        // nếu có (SupportsGet = true) nên Account sẽ luôn khác null nên ModelState luôn kích hoạt
        // còn model không có (SupportsGet = true) thì ModelState chỉ kích hoạt khi có param truyền vào (model khác null)
        // nếu model khác null  và trong model có anotation validation thì nó sẽ check validation luôn 
        // có (SupportsGet = true) thì model khác null khi trỏ vào cotroller thì nó khác null nên check anotation validation nếu có lỗi thì trả lỗi khi lần đầu load page
        // nếu model null khi trỏ vào controller thì chỉ khi có param truyền vào thì model mới khác null và lúc đó mới check validation
        // khi chỉ truyền model Movie về thì tất cả model BindProperty khác đều khởi tạo
        // .net 6 trở lên mặc định là check [Required] string , còn .net5 thì không cho nên phải tự thêm [Required]
        if (!ModelState.IsValid)
        {
            return View();

        }
        return Json($"ModelState Valid");
        return Json(true);
    }

    public IActionResult ClearValidation([Bind("Id,Name,ShortName,Email,PhoneNumber")][RegularExpression(@"^\d{3}-\d{3}-\d{4}$")][BindRequired, FromQuery] string phone)
    {
        // bỏ qua validate thì
        ModelState.Remove("Account.Nam");
        // hoặc [ValidateNever] hoặc thêm dấu ? vào sau kiểu dữ liệu => public string? Email { get; set; }

        // xóa hết 
        //ModelState.Clear();
        ModelState.AddModelError("Contact.ShortName", "Short name can't be the same as Name.");
        ModelState.AddModelError(nameof(Account.Name), "Short name can't be the same as Name.");

        if (Account.Name == Account.Avatar)
        {
            ModelState.AddModelError("Contact.ShortName",
                                     "Short name can't be the same as Name.");
        }

        //if (_context.Contact.Any(i => i.PhoneNumber == Contact.PhoneNumber))
        //{
        //    ModelState.AddModelError("Contact.PhoneNumber",
        //                              "The Phone number is already in use.");
        //}
        //if (_context.Contact.Any(i => i.Email == Contact.Email))
        //{
        //    ModelState.AddModelError("Contact.Email", "The Email is already in use.");
        //}

        Account.Name = "assssssss";
        // clear validate 1 model binding trong class
        ModelState.ClearValidationState(nameof(Account));
        //  validate lai 1 model da sua trong function (Account.Name = "assssssss";)
        if (!TryValidateModel(Account, nameof(Account)))
        {
            return View();
        }

        ModelState.AddModelError("", "Unable to save changes. " +
      "Try again, and if the problem persists " +
      "see your system administrator.");

        // check validate tất cả model binding trong class và trong ctor properties
        if (!ModelState.IsValid)
        {
            return View();
        }
        return View();
    }

    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyEmail(string email)
    {
        //if (!_userService.VerifyEmail(email))
        //{
        //    return Json($"Email {email} is already in use.");
        //}

        if (_eTransportationSystemContext.Accounts.Any(a => a.Email.Equals(email)))
        {
            return Json($"Email {email} is already in use.");
        }

        return Json(true);
    }

    public IActionResult ModelStateRedirect()
    {
        TempData["Message"] = "Error Error Error Error ErrorErrorError";
        return RedirectToAction("ModelStateRedirectReceiver");
    }

    public IActionResult ModelStateRedirectReceiver()
    {
        string message = TempData["Message"] as string;
        ModelState.AddModelError("Error", message);
        return View("Index");
    }

}
