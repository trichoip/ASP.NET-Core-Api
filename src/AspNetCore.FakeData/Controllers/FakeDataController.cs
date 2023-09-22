using AutoBogus;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.FakeData.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FakeDataController : ControllerBase
    {

        [HttpGet]
        public IActionResult Bogus()
        {
            var address = new Faker<Address>()
                .RuleFor(a => a.Line1, f => f.Address.BuildingNumber())
                .RuleFor(a => a.Line2, f => f.Address.StreetAddress())
                .RuleFor(a => a.PinCode, f => f.Address.ZipCode());

            var customer = new Faker<Customer>()
                .RuleFor(c => c.Id, f => f.Random.Number(1, 100))
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.Email, f => f.Person.Email)
                .RuleFor(c => c.Bio, f => f.Lorem.Paragraph(1))
                .RuleFor(c => c.Address, f => address.Generate()) // do  address chưa gennerate nên mới phải gọi address.Generate() mới trả về data
                .Generate(); // Generate là lấy ra object, nếu không thì vẫn là Faker như address ơ trên 

            var address2 = new Faker<Address>()
                    .RuleForType(typeof(string), c => c.Random.AlphaNumeric(50))
                    .Generate();

            var customer2 = new Faker<Customer>()
                    .RuleForType(typeof(string), c => c.Random.Word())
                    .RuleForType(typeof(int), c => c.Random.Number(10, 20))
                    .RuleFor(c => c.Email, f => f.Person.Email)
                    .RuleFor(c => c.Address, f => address.Generate())
                    .Generate();

            return Ok(new
            {
                address = address.Generate(),
                customer,
                address2,
                customer2,
                addressList = address.Generate(3), // nếu Generate mà truyền vào số thì sẽ trả về list
                addressListOne = address.Generate(1), // nếu Generate mà truyền vào số thì sẽ trả về list
                addressListGenerateForever = address.GenerateForever().Take(2),// nếu không Take() thì nó sẽ chạy mãi mãi
            });
        }

        [HttpGet]
        public IActionResult BogusRandomizer()
        {
            Randomizer.Seed = new Random(110);
            // nếu cấu hình Randomizer.Seed thì khi trả về data sẽ giống nhau
            // ví dụ : nếu ở dưới trả về là 1 thì khi chạy lần nữa nó vẫn là 1 
            // nếu thêm rule hay thêm gì đó thì nó sẽ thay đổi hoặc thảy đổi số Random(110);

            var address = new Faker<Address>()
                .RuleFor(a => a.Line1, f => f.Address.BuildingNumber())
                .RuleFor(a => a.Line2, f => f.Address.StreetAddress())
                .RuleFor(a => a.PinCode, f => f.Address.ZipCode());

            return Ok(address.Generate());
        }

        [HttpGet]
        public IActionResult AutoBogus()
        {
            var customer = AutoFaker.Generate<Customer>();

            return Ok(customer);
        }

        [HttpGet]
        public IActionResult FakerNet()
        {
            return Ok(Faker.Address.StreetName());
        }

    }

    public class Customer
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public Address Address { get; set; }
    }
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PinCode { get; set; }
    }
}