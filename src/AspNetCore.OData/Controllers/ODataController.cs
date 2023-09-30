using AspNetCore.OData.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace AspNetCore.OData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ODataController : ControllerBase
    {
        private readonly MyWorldDbContext _myWorldDbContext;
        public ODataController(MyWorldDbContext myWorldDbContext)
        {
            _myWorldDbContext = myWorldDbContext;
        }

        /// <remarks>
        ///  ```
        ///    Select: https://localhost:7124/OData?$select=id,firstname
        ///    Filter: https://localhost:7124/OData?$filter=salary gt 1000000
        ///    OrderBy: https://localhost:7124/OData?$orderby=id desc
        ///    Skip: https://localhost:7124/OData?$skip=13
        ///    Top: https://localhost:7124/OData?$top=3
        ///    Expand: https://localhost:7124/OData?$expand=backpack($select=id),factions($select=id),weapons($select=id)
        ///  ```
        /// </remarks>
        [HttpGet]
        [EnableQuery]
        public IActionResult Get([FromQuery] oDataField oData)
        {
            return Ok(_myWorldDbContext.Employee); // giống bên dưới
            return Ok(_myWorldDbContext.Employee.AsQueryable());
        }

        [HttpPost]
        [EnableQuery]
        public IActionResult Post([FromBody] oDataField data)
        {
            return Ok();
        }

        [HttpGet("list")]
        [EnableQuery]
        public IActionResult ToList(string? select)
        {
            // nếu là tolist thì nó sẽ returrn query hết tất cả properties sau đó lọc theo điều kiện của OData
            // xem log là rõ
            // cho nên nếu lọc properties trong query thì không được tolist() mà phải dùng AsQueryable() hoặc Dbset<>
            return Ok(_myWorldDbContext.Employee.ToList());
        }

        public class oDataField
        {

            /// <summary >
            ///  ```
            ///    - Select: https://localhost:7124/OData?$select=id,firstname  
            ///    - $select=id,firstname   
            ///  ```
            /// </summary>
            /// <example>id,firstname</example>
            public string? select { get; set; }

            /// <summary >
            ///  ```
            ///    - Filter: https://localhost:7124/OData?$filter=salary gt 1000000  
            ///    - $filter=salary gt 1000000  
            ///    - eq  equals to  
            ///    - ne not equals to  
            ///    - gt  greater than   
            ///    - ge   greater than or equal  
            ///    - lt  less than  
            ///    - le  less than or equal   
            ///    - add, sub, mult, div, mod
            ///    - ()
            ///    - null
            ///    - substringof, endswith, startswith, length, indexof, substring, tolower, toupper, trim, concat
            ///    - year(), month(), day(), hour(), minute(), second()
            ///    - round(), floor(), ceiling()
            ///    - isof()
            ///  ```
            /// </summary>
            /// <example>salary gt 1000000</example>
            public string? filter { get; set; }

            /// <summary >
            ///  ```
            ///    - OrderBy: https://localhost:7124/OData?$orderby=id desc
            ///    - $orderby=id desc
            ///  ```
            /// </summary>
            /// <example>id desc</example>
            public string? orderby { get; set; }

            /// <summary >
            ///  ```
            ///    - Skip: https://localhost:7124/OData?$skip=13
            ///    - $skip=13
            ///  ```
            /// </summary>
            /// <example>13</example>
            public string? skip { get; set; }

            /// <summary >
            ///  ```
            ///    - Top: https://localhost:7124/OData?$top=3
            ///    - $top=3
            ///  ```
            /// </summary>
            /// <example>3</example>
            public string? top { get; set; }

            /// <summary >
            ///  ```
            ///    - Expand: https://localhost:7124/OData?$expand=backpack($select=id),factions($select=id),weapons($select=id)
            ///    - $expand=backpack
            ///    - $expand=backpack($select=id)
            ///    - $expand=backpack,factions,weapons
            ///    - $expand=backpack($select=id),factions($select=id),weapons($select=id)
            ///  ```
            /// </summary>
            /// <example>backpack($select=id)</example>
            public string? expand { get; set; }

            public string? count { get; set; }
        }

    }
}