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
