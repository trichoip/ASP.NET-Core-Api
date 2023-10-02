using AspNetCore.Mapper.Config;
using AspNetCore.Mapper.Models;
using AspNetCore.Mapper.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AspNetCore.Mapper.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MapperController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMapper mapperStatic;
        private readonly IMapper mapperStatic2;
        private readonly IMapper mapperDirectly;
        private static List<Driver> drivers = new List<Driver>();

        #region Ctor
        public MapperController(IMapper mapper)
        {
            // cách 1 : sử dụng DI
            // lưu ý : phải có cấu hình này builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // và những <Class> : Profile thì tự động được inject vào DI, ví dụ như class DriverProfile
            _mapper = mapper;

            // cách 2 : sử dụng static
            mapperStatic = MapperConfig.InitializeAutomapper();

            // cách 2.1 : sử dụng static khác
            var config = new MapperConfiguration(cfg =>
            {
                MapperConfig.CreateMapProduct(cfg);
                // có thể add cấu hình khác ở đây
            });
            mapperStatic2 = config.CreateMapper();

            // cách 3 : sử dụng trực tiếp
            var configDirectly = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>()
                    .ForMember(dest => dest.OptionalName, opt => opt.NullSubstitute("config Directly"))
                    .ReverseMap();
            });
            mapperDirectly = configDirectly.CreateMapper();
        }
        #endregion

        #region FindAllDriver
        [HttpGet]
        public IActionResult FindAllDriver()
        {
            // mapper list to list
            var driverDtoList = _mapper.Map<IEnumerable<DriverDto>>(drivers);
            return Ok(new { driverDtoList });
        }
        #endregion

        #region ProjectTo Map IQueryable
        [HttpGet]
        public IActionResult ProjectTo()
        {
            IQueryable<Driver>? driversIQ = null;
            var driverDtoList = _mapper.ProjectTo<DriverDto>(driversIQ);
            return Ok(new { driverDtoList });
        }
        #endregion

        #region CreateDriver
        [HttpPost]
        public IActionResult CreateDriver(DriverDto data)
        {
            var _driver = _mapper.Map<Driver>(data);

            var newDriver = new Driver()
            {
                FirstName = "new driver",
            };

            // do có cấu hình ReverseMap() nên có thể mapper ngược lại từ Driver sang DriverDto
            // và fullname map từ FirstName
            var driverDto = _mapper.Map<Driver, DriverDto>(newDriver);

            drivers.Add(_driver);
            return Ok(new { _driver, driverDto });
        }
        #endregion

        #region FindOneEmloyee
        [HttpGet]
        public IActionResult FindOneEmloyee()
        {

            Employee emp = new Employee
            {
                Name = "James",
                Salary = 20000,
                Address = new Address()
                {
                    City = "Mumbai",
                    Number = 3
                }
            };

            EmployeeDTO emp1 = new EmployeeDTO
            {
                Name = "EmployeeDTO1",
                Salary = 111,
                AddressDTO = new AddressDTO()
                {
                    City = "AddressDTO1",
                    Number = 222
                }
            };

            // có nhiều cách viết tương đương nhau
            // mapper with new object
            var empDTO1 = _mapper.Map<EmployeeDTO>(emp);
            var empDTO1_1 = _mapper.Map<Employee, EmployeeDTO>(emp);
            var empDTO1_2 = _mapper.Map(emp, typeof(Employee), typeof(EmployeeDTO));

            // mapper with exists object
            // lưu ý : nếu mapper exists thì không cần khai bao kiểu dữ liệu vì nó mapper vào biến sẵn có trước đó
            // nếu mapper mà khai báo như dòng dưới thì empDTO3_1 chính là emp, vì vậy không cần khai báo kiểu dữ liệu
            var empDTO3_1 = _mapper.Map<EmployeeDTO, Employee>(emp1, emp);
            _mapper.Map(emp1, emp, typeof(EmployeeDTO), typeof(Employee));
            _mapper.Map(emp1, emp);

            return Ok(new { empDTO1_2, emp });
        }
        #endregion

        #region FindOneOrder
        [HttpGet]
        public IActionResult FindOneOrder()
        {
            var Order = new Order
            {
                OrderNo = 101,
                NumberOfItems = 3,
                TotalAmount = 1000,
                Customer = new()
                {
                    CustomerID = 777,
                    FullName = "James Smith",
                    Postcode = "755019",
                    ContactNo = "1234567890"
                }
            };

            var orderDTO = new OrderDTO
            {
                OrderId = 2,
                NumberOfItems = 2,
                TotalAmount = 2,
                CustomerId = 55,
                Name = "OrderDTO",
                Postcode = "OrderDTO",
                MobileNo = "OrderDTO"

            };

            var orderDTOData = _mapper.Map<OrderDTO>(Order);

            _mapper.Map(orderDTO, Order);

            return Ok(new { orderDTOData, Order });
        }
        #endregion

        #region CreateProduct
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product, [FromQuery, DefaultValue("v")] String name)
        {
            var prdDto = _mapper.Map<ProductDTO>(product);

            var prd = _mapper.Map<Product>(new ProductDTO { ItemName = name });

            return Ok(new { prdDto, prd });
        }
        #endregion

        #region OtherConfigAutomapper
        [HttpPut]
        public IActionResult OtherConfigAutomapper(Product product)
        {
            // cách 1 : sử dụng DI
            var DI = _mapper.Map<ProductDTO>(product);

            // cách 2 : sử dụng static
            var Static = mapperStatic.Map<ProductDTO>(product);

            // cách 2.1 : sử dụng static khác
            var Static2 = mapperStatic2.Map<ProductDTO>(product);

            // cách 3 : cấu hình trực tiếp
            var Directly = mapperDirectly.Map<ProductDTO>(product);

            return Ok(new { DI, Static, Static2, Directly });
        }
        #endregion

    }
}
