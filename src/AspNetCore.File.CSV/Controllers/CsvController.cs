using AspNetCore.File.CSV.ClassMapsCsv;
using AspNetCore.File.CSV.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AspNetCore.File.CSV.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CsvController : ControllerBase
    {
        [HttpGet]
        public IActionResult CsvRead()
        {
            // lưu ý properties trên header phải giống với properties của class kể cả chữ hoa, thường
            // nếu không muốn giống thì dùng PrepareHeaderForMatch = args => args.Header.ToLower()
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                //HasHeaderRecord = false, // nếu file csv không có header thì thêm hàng dưới
                //Encoding = Encoding.UTF8, // File của ta dùng encoding UTF-8.
                //Delimiter = "," // Ký tự phân cách giữa các trường là dấu phẩy.
            };

            List<Foo> foos;
            using (var reader = new StreamReader("wwwroot\\data.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Foo>();
                foos = records.ToList();
            }

            return Ok(foos);
        }

        [HttpGet]
        public IActionResult CsvRead2()
        {

            List<Foo> foos;
            using (var reader = new StreamReader("wwwroot\\data.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<FooMap>();
                foos = csv.GetRecords<Foo>().ToList();
            }

            return Ok(foos);
        }

        [HttpPost]
        public IActionResult CsvWrite(List<Foo> records)
        {
            using (var writer = new StreamWriter("wwwroot\\write.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<FooMap>();
                csv.WriteRecords(records);
            }

            return Ok();
        }
    }

}
