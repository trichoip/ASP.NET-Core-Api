using AspNetCore.Api.Models;
using AspNetCore.Api.Record;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModelBindingController : ControllerBase
    {

        [HttpGet("{id}")]
        // nếu có [ApiController] -> mà nhập id là abc thì sẽ trả về error json  400 Bad Request
        // nếu không có [ApiController] -> mà nhập id là abc thì vẫn sẽ vô được hàm và id = 0
        // [ApiController] giúp tự check value , nếu value không hợp lệ thì trả về 400 Bad Request
        // nếu không có [ApiController] thì phải tự check value
        // [ApiController] giúp tự động validate model
        // [ApiController] giúp tự động trả về 400 Bad Request khi model không hợp lệ

        // - không liên quan đến [ApiController]
        // {id?} nếu có dấu ? thì id có thể null, là có thể không truyền id mà vẫn call được hàm 
        // {id} nếu không có dấu ? thì id không thể null, là phải truyền id mới call được hàm nếu không sẽ bị lỗi 404 Not Found
        public IActionResult GetById(int id, bool dogsOnly) // (int? id) nếu id không truyền về thì id = null | (int id) nếu id không truyền về thì id = 0
        {
            return Ok(new { id, mes = "id not allow null" });
        }

        //[HttpGet("int/{id:int?}")]
        [HttpGet("int/{id:int:min(5)?}")]
        // => int? là cho phép không truyên id, nếu không có ? thì bắt buộc phải truyền path variable id => /id
        // không nên dùng {id:int} vì khi truyền id = abc thì  dù có [ApiController] hay không vẫn trả về 404 Not Found chứ không trả về 400 Bad Request
        public IActionResult About(int? id)
        {
            return Ok(new { id, mes = "id allow null" });
        }

        [HttpGet]
        public ActionResult FromHeader([FromHeader(Name = "Accept-Language")] string language,
                                       [FromHeader] string countries)
        {
            return Ok(new { language, countries });
        }

        [HttpGet]
        public ActionResult FromQuery([FromQuery] TodoItem todoItem)
        {
            return Ok(new { todoItem });
        }

        [HttpPut]
        public ActionResult FromBody([FromBody] TodoItem todoItem)
        {

            //[ApiController] đã tự động validate model nên không cần check
            //if (!ModelState.IsValid)
            //{
            //    return null;
            //}

            return Ok(new { todoItem });
        }

        [HttpPut]
        public ActionResult FromForm([FromForm] TodoItem todoItem)
        {
            return Ok(new { todoItem });
        }

        [HttpPut]
        public ActionResult IFormFile(IFormFile file1, IEnumerable<IFormFile> file2)
        {

            return Ok(new { file1, file2 });
        }

        [HttpPost]
        public ActionResult Record(Person person)
        {
            return Ok(new { person });
        }

    }
}
