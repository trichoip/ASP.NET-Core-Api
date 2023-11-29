using AspNetCore.MappingObject.AutoMapper.Models;
using AspNetCore.MappingObject.AutoMapper.Profiles;
using AspNetCore.MappingObject.AutoMapper.ViewModel;
using AutoMapper;

namespace AspNetCore.MappingObject.AutoMapper.Config
{
    public class MapperConfig
    {
        public static IMapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>()
                    .ForMember(dest => dest.OptionalName, opt => opt.NullSubstitute("Config method Static InitializeAutomapper"))
                    .ReverseMap();
                // có thể add cấu hình khác ở đây
            });
            var mapper = config.CreateMapper();
            return mapper;
        }

        public static void CreateMapProduct(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Product, ProductDTO>()
                    .ForMember(dest => dest.OptionalName, opt => opt.NullSubstitute("Config method Static CreateMapProduct"))
                    .ReverseMap();
        }

        //  AutoMapper.Extensions.Microsoft.DependencyInjection
        // extension trên giúp cấu hình profile vào DI IMapper cho nên không cần cấu hình AddProfile nữa 
        // class kế thừa Profile là class cấu hình cho AutoMapper DI
        public static IMapper InitializeAutomapperProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DriverProfile());
            });
            var mapper = config.CreateMapper();
            return mapper;
        }

        public static void AddProfileProduct(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile(new DriverProfile());
        }
    }

}
