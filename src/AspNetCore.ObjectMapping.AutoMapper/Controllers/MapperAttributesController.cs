using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.ObjectMapping.AutoMapper.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MapperAttributesController : ControllerBase
{
    private readonly IMapper _mapper;
    public MapperAttributesController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Get(PeoDto peoDto)
    {
        var peo = _mapper.Map<Peo>(peoDto);

        var peoDto2 = _mapper.Map<PeoDto>(peo);

        return Ok(new { peo, peoDto2 });
    }
}

// PeoDto -> Peo
//[AutoMap(typeof(PeoDto))]
public class Peo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
}

// Peo -> PeoDto
[AutoMap(typeof(Peo), ReverseMap = true)]
public class PeoDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    //[Ignore]
    public string Color { get; set; }

    [SourceMember(nameof(Peo.Description))]
    public string DescriptionNew { get; set; }
}