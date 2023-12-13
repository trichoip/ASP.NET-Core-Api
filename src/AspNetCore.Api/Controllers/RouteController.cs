using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers;

[Route("api/[controller]/[action]", Name = "[controller]_[action]")]
[ApiController]
public class RouteController : ControllerBase  // nên dùng khi viết api, còn nếu viết cả view thì dung Controller , Controller kế thừa từ ControllerBase
{

    [Route("")]
    [Route("/")] //  [Route("~/")]
    [Route("Home")]
    [Route("Home/Index")]
    [Route("Home/Index/{id?}")]
    [Route("[controller]/[action]")]
    [Route("[controller]/[action]/{id?}")]
    [HttpPut]
    // MultipleRoute(int id) nếu cấu hình path variable id là các Route không có path variable thì không thể truyển id query được
    // nếu MultipleRoute(int id) không cấu hình path variable id thì các Route sẽ có thể truyền param query id
    // nhìn api MultipleRoute(int id) và MultipleHttp(int id) là rõ

    // nếu cấu hình Rount hoặc api bắt buộc phải có http attribute để swagger nhận dạng được
    // nếu không có thì lỗi hiển thị swagger nhưng api vẫn call bth
    // nếu không có http attribute thì có thể call api bằng http method nào cũng được grt , put , post , delete , patch , ...

    // khi đặt tên route trong http attribute thì không thể dùng [Route("name")] để đặt tên ,
    // nếu không đặt tên trong http attribute thì có thể dùng nhiều [Route("name")] để đặt tên
    // lỗi hiển thị swagger nếu không tuân thủ các trường hợp trên (chỉ lỗi hiển thị swagger chứ api vẫn call bth):
    public IActionResult MultipleRoute(int id)
    {
        return Ok();
    }

    [HttpPost("Checkout")]
    [HttpPost("Checkin")]
    // không cấu hình path variable id thì các Route trên sẽ có thể truyền param query id
    // nếu cấu hình path variable id ([HttpPost("Checkin/{id}")]) thì Route [HttpPost("Checkout")] sẽ không thể truyền param query id
    public IActionResult MultipleHttp(int id)
    {
        return Ok();
    }

    [HttpPatch]
    [ActionName("ActionPatch")]
    //[Route("{EmployeeName: alpha}")]
    //[Route("{EmployeeId:int:min(100):max(1000)}")]
    //[Route("{EmployeeId:int:range(100,1000)}")]
    //[Route("{EmployeeName:alpha:minlength(5)}")]
    //[Route("{EmployeeName:regex(list(b|c))}")]
    public IActionResult ActionName()
    {
        return Ok();
    }

}
