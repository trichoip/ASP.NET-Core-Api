using AspNetCore.Client.OData.Data;
using AspNetCore.Client.OData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace AspNetCore.Client.OData.Controllers;


public class EmployeeODataTestController : ControllerBase
{

    private readonly MyWorldDbContext _myWorldDbContext;
    public EmployeeODataTestController(MyWorldDbContext myWorldDbContext)
    {
        _myWorldDbContext = myWorldDbContext;
    }

    [EnableQuery]
    public IActionResult Get() // phải đặt đúng tên là httpmethod Get,Post,Put,Delete,... 
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
