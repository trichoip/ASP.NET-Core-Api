using AspNetCore.ObjectMapping.AutoMapper.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.ObjectMapping.AutoMapper.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class MapperInterfaceController : ControllerBase
{
    private readonly IMapper _mapper;
    public MapperInterfaceController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Get(CarDTO carDTO)
    {
        var car = _mapper.Map<Car>(carDTO);

        var carDTO2 = _mapper.Map<CarDTO>(car);

        return Ok(new { car, carDTO2 });
    }

}

public class Car
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
}

public class CarDTO : IMapFrom<Car>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
}