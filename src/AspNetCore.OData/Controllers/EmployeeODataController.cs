using AspNetCore.OData.Data;
using AspNetCore.OData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace AspNetCore.OData.Controllers
{
    public class EmployeeODataController : Microsoft.AspNetCore.OData.Routing.Controllers.ODataController
    {

        private readonly MyWorldDbContext _myWorldDbContext;
        public EmployeeODataController(MyWorldDbContext myWorldDbContext)
        {

            _myWorldDbContext = myWorldDbContext;
        }

        [EnableQuery(PageSize = 1)]
        public IActionResult Get()
        {
            // nếu muốn dùng odata select,filter,orderby,expand,... thì phải có cái này  [EnableQuery]
            // post create trả về object vẫn dùng odata được nhưng phải có [EnableQuery]

            // lưu ý: nếu dùng api odata được cấu hình trong program.cs để select,filter,orderby,expand,...
            // thì object trả về phải trùng với object được cấu hình EntitySet trong program.cs nếu không sẽ bị lỗi
            // ví dụ: trong program.cs có cấu hình EntitySet<Employee>("Employees");
            // thì object trả về phải là Employee hoặc List<Employee>
            // nếu không dùng select,filter,orderby,expand,... thì không có lỗi gì cả, còn nếu dùng thì phải trùng object

            // ví dụ 2: nếu post trả về object là Employee thì phải có [EnableQuery] mới dùng được select,filter,orderby,expand,...
            // còn post mà trả về EmployeeDto mà dùng select,filter,orderby,expand,... thì sẽ bị lỗi, còn không dùng thì không có lỗi gì cả

            return Ok(_myWorldDbContext.Employee.AsQueryable());
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_myWorldDbContext.Employee.FirstOrDefault(c => c.Id == key));
        }

        [EnableQuery]
        public IActionResult Post([FromBody] Employee employee)
        {
            _myWorldDbContext.Employee.Add(employee);
            _myWorldDbContext.SaveChanges();

            return Ok(employee);
        }

        [EnableQuery]
        public IActionResult Delete(int key)
        {
            var employee = _myWorldDbContext.Employee.Find(key);
            if (employee is null) return NotFound();

            _myWorldDbContext.Employee.Remove(employee);
            _myWorldDbContext.SaveChanges();
            return Ok();
        }

        [EnableQuery]
        public IActionResult Put(int key, [FromBody] Employee employee)
        {
            return Ok();
        }
    }
}
