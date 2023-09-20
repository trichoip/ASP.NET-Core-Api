using AspNetCore.Mapper.Models;
using AspNetCore.Mapper.ViewModel;
using AutoMapper;

namespace AspNetCore.Mapper.Profiles
{

    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            // ctrl m m để đóng mở region
            // Expand -  CTRL+R, CTRL + Num +
            // Collapse -  CTRL + R, CTRL + Num -
            // xml -  (Ctrl+E, Ctrl+Num+) and (Ctrl+E, Ctrl+Num-).
            #region Driver, DriverDtom
            // lưu ý: nếu mapper father1 có child1 qua father2 có child2 thì phải cáu hình CreateMap của cả father và child
            // CreateMap<DriverDto, Driver>() chỉ cần vậy là có thể mapper List<DriverDto> sang List<Driver>

            CreateMap<DriverDto, Driver>() // lúc đầu <DriverDto, Driver>
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Addresses.Select(le => le.Number).Sum()))
                .ReverseMap(); // ReverseMap giúp mapper ngược lại từ Driver sang DriverDto và fullname map từ FirstName, hàm bên dưới (1) là ReverseMap() cùa hàm này
                               // ReverseMap không chỉ mapper từ Driver sang DriverDto mà còn mapper ngược lại ForMember nữa,
                               // ví du: FirstName MapFrom FullName thì ReverseMap giúp mapper FullName MapFrom FirstName
                               // lưu ý: sau ReverseMap là <Driver, DriverDto>

            // (1) bên trên đã có ReverseMap nên không cần mapper ngược lại
            // CreateMap<Driver, DriverDto>()
            //	.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName));
            #endregion

            #region Address, AddressDTO
            // lưu ý: nếu không có ReverseMap() thì phải viết 2 hàm CreateMap như bên dưới
            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>();
            #endregion

            #region Employee, EmployeeDTO , Condition
            // lưu ý: nó chỉ mapper Properties có cùng Name, nếu không có cùng Name mà muốn mapper thì phải dùng ForMember để mapper như bên dưới
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.AddressDTO, opt => opt.MapFrom(src => src.Address))
                .ReverseMap(); // <EmployeeDTO, Employee>

            #endregion

            #region Order and OrderDTO , ReverseMap()
            // nếu có trường hợp như Order và OrderDTO => trong Order có Customer và trong OrderDTO không có Customer mà có properties của Customer
            // xem trong class Order, OrderDTO để hiểu rõ hơn
            // nếu có trường hợp như vậy thì nên mapper theo chiều hướng như cách 1 (Order -> OrderDTO), vì cách 1 sẽ giúp mapper ngược lại dễ dàng hơn và không cấn phải cấu hình mapper ngược lại như cách 2

            // cách 1 và 2 tương đương nhau , chỉ khác nhau ở thứ tự dest và src , chỉ chọn cách 1 hoặc cách 2, không nên chọn cả 2 cách 

            // cách 1: 
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.CustomerID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.Customer.ContactNo))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(src => src.Customer.Postcode))
                .ReverseMap();

            // cách 2: không nên sài cách này
            CreateMap<OrderDTO, Order>() // lúc đầu <OrderDTO, Order>
                .ForMember(dest => dest.OrderNo, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new Customer()
                {
                    CustomerID = src.CustomerId,
                    FullName = src.Name,
                    Postcode = src.Postcode,
                    ContactNo = src.MobileNo
                }))
                .ReverseMap() // sau OrderseMap là <Order, OrderDTO>
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.CustomerID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.Customer.ContactNo))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(src => src.Customer.Postcode));

            #endregion

            #region Product, ProductDTO
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Name.StartsWith("a") ? src.Name : "not StartsWith(a)"))
                // Condition nếu không thỏa mãn thì không mapper và value là default , nếu thỏa mãn thì mới mapper
                // luư ý: nếu properties khác name thì phải có MapFrom thì Condition mới có tác dụng, như ItemQuantity <=> Quantity
                .ForMember(dest => dest.ItemQuantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ItemQuantity, opt => opt.Condition(src => src.Quantity >= 2))
                .ForMember(dest => dest.Amount, opt => opt.Condition(src => src.Amount >= 10))
                .ForMember(dest => dest.Igone, opt => opt.Ignore())
                .ForMember(dest => dest.OptionalName, opt => opt.NullSubstitute("N/A")) // nếu null thì value là "N/A"
                .ForMember(dest => dest.Moon, opt => opt.DoNotAllowNull()) // nếu null thì value là empty
                                                                           //
                .ForMember(dest => dest.CreatedOn, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.CreatedBy, opt => opt.UseDestinationValue())
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ItemName.StartsWith("b") ? src.ItemName : "not StartsWith(b)"));
            #endregion

        }

    }
}
