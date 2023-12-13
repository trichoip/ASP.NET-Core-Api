using AspNetCore.Api.HandleError.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.HandleError.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class HandleErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult Throw_DemoException()
    {
        throw new DemoException("DemoException_AddProblemDetails");
    }

    [HttpGet]
    public IActionResult Throw_Demo2Exception()
    {
        throw new Demo2Exception("Throw_Demo2Exception");
    }

    [HttpGet]
    public IActionResult Throw_ArgumentException()
    {
        throw new ArgumentException("ArgumentException");
    }

    [HttpGet]
    public IActionResult Throw_Exception()
    {
        throw new Exception("Exception");
    }

    [HttpGet]
    public IActionResult Error_NotFound()
    {
        return NotFound();
    }

    #region Handle error cach 3

    [HttpGet]
    public IActionResult Throw_KeyNotFoundException()
    {
        throw new KeyNotFoundException("KeyNotFoundException");
    }

    [HttpGet]
    public IActionResult Throw_AppException()
    {
        throw new AppException("AppException");
    }
    #endregion

    #region Handle error cach 2
    [HttpGet]
    public IActionResult Throw_HttpResponseException()
    {
        throw new HttpResponseException(406, "HttpResponseException hello");
    }
    #endregion

}
