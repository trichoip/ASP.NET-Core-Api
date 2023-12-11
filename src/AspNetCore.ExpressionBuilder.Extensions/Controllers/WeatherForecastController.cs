using AspNetCore.ExpressionBuilder.Extensions.Data;
using AspNetCore.ExpressionBuilder.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.ExpressionBuilder.Extensions.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WeatherForecastController(ApplicationDbContext context)
    {
        _context = context;
    }

    // lưu ý: or là false , and là true
    // Or trước sai thì mới chạy điều kiện sau
    // vì or thì điều kiện đầu sai thì nó mới chạy điều kiện thứ 2
    // và điều kiện thứ 2 là false thì chạy điều kiện thứ 3, cho tới khi nào true thì dừng

    // còn And thì trước đúng thì mới chạy điều kiện sau

    [HttpGet]
    public IActionResult PredicateBuilderDemo()
    {

        var predicate = PredicateBuilder.False<WeatherForecast>();
        predicate = predicate.Or(x => x.TemperatureC >= 20);
        predicate = predicate.Or(x => x.TemperatureF <= 10);

        var predicateAnd = PredicateBuilder.True<WeatherForecast>();
        predicateAnd = predicateAnd.And(x => x.Id >= 20);
        predicateAnd = predicateAnd.And(x => x.Id < 5);

        predicate = predicate.And(predicateAnd);

        var entity = _context.WeatherForecasts.Where(predicate);

        return Ok(entity.ToQueryString());
    }

    [HttpGet]
    public IActionResult ExpressionExtensions()
    {
        var predicate = PredicateBuilder.False<WeatherForecast>();
        predicate = predicate.OrElse(x => x.TemperatureC >= 20);
        predicate = predicate.OrElse(x => x.TemperatureF <= 10);

        var predicateAnd = PredicateBuilder.True<WeatherForecast>();
        predicateAnd = predicateAnd.AndAlso(x => x.Id >= 20);
        predicateAnd = predicateAnd.AndAlso(x => x.Id < 5);

        predicate = predicate.AndAlso(predicateAnd);

        var entity = _context.WeatherForecasts.Where(predicate);

        return Ok(entity.ToQueryString());
    }
}
