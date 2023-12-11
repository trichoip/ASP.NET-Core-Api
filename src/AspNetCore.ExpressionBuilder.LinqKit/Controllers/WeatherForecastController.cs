using AspNetCore.ExpressionBuilder.LinqKit.Data;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCore.ExpressionBuilder.LinqKit.Controllers;
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

        var predicate = PredicateBuilder.New<WeatherForecast>();
        predicate.Or(x => x.TemperatureC >= 20);
        predicate.Or(x => x.TemperatureF <= 10);

        var predicateAnd = PredicateBuilder.New<WeatherForecast>();
        predicateAnd.Or(x => x.Id >= 20);
        predicateAnd.Or(x => x.Id < 5);

        predicate.And(predicateAnd);

        var entity = _context.WeatherForecasts.Where(predicate);

        return Ok(entity.ToQueryString());
    }

    [HttpGet]
    public IActionResult PredicateBuilderDemo2()
    {

        var predicate = PredicateBuilder.New<WeatherForecast>();
        predicate.And(x => x.TemperatureC >= 20);
        predicate.And(x => x.TemperatureF <= 10);

        var predicate2 = PredicateBuilder.New<WeatherForecast>();

        predicate2.Or(x => x.Id >= 20);
        predicate2.Or(x => x.Id < 5);

        predicate.And(predicate2);

        var entity = _context.WeatherForecasts.Where(predicate);

        return Ok(entity.ToQueryString());
    }

    [HttpGet]
    public IActionResult CompareFuncAndExpr()
    {
        Func<Pop, bool> func = x => x.Id > 2;
        Expression<Func<Pop, bool>> expr = x => x.Id > 2;

        // xem log để thấy sự khác nhau

        // func thì khi call nó query lấy hết data từ database về bộ nhớ rồi mới where
        // expr thì khi call nó query và where từ database sau đó trả về data phù hợp với điều kiện where
        var successFunc = _context.Pops.Any(func);
        var successExpr = _context.Pops.Any(expr);

        return Ok(new
        {
            successFunc,
            successExpr,
        });

    }

    [HttpGet]
    public IActionResult PluggingExpressions_Error()
    {
        Func<Pop, bool> func = x => x.Id > 2;
        // Khi run sẽ bị lỗi, vì không thể truyền func vào Any được mà phải viết trực tiếp vào Any
        var error = _context.WeatherForecasts.Where(_ => _.TemperatureC > 20 && _.Pops.Any(func));
        var success = _context.WeatherForecasts.Where(_ => _.TemperatureC > 20 && _.Pops.Any(x => x.Id > 2));

        return Ok(new
        {
            error = error.ToQueryString(),
            success = success.ToQueryString(),
        });

    }

    [HttpGet]
    public IActionResult PluggingExpressions_Solution()
    {
        Expression<Func<Pop, bool>> expr = x => x.Id > 2;

        var entity = _context.WeatherForecasts
            .AsExpandable()
            .Where(_ => _.TemperatureC > 20 && _.Pops.Any(expr.Compile()));

        return Ok(entity.ToQueryString());

    }

    [HttpGet]
    public IActionResult PluggingExpressions_Solution2()
    {
        Expression<Func<Pop, bool>> expr = x => x.Id > 2;

        var predicate = PredicateBuilder.New<WeatherForecast>();
        predicate.And(x => x.TemperatureC >= 20);
        predicate.And(x => x.TemperatureF <= 10);
        predicate.And(_ => _.Pops.Any(expr.Compile()));

        var entity = _context.WeatherForecasts.AsExpandable().Where(predicate);

        return Ok(entity.ToQueryString());

    }

    [HttpGet]
    public IActionResult Subqueries()
    {
        Expression<Func<Pop, bool>> expr = x => x.Id > 2;

        var entity = from w in _context.WeatherForecasts.AsExpandable()
                     let pops = _context.Pops.Where(_ => _.WeatherForecastId == w.Id).ToList()
                     where w.Pops.Select(_ => _.Id).Contains(w.Id) && pops.Any(expr.Compile())
                     select w.Summary;

        return Ok(entity.ToQueryString());

    }

    [HttpGet]
    public IActionResult CombiningExpressions()
    {
        Expression<Func<WeatherForecast, bool>> expr1 = x => x.Id > 2;

        Expression<Func<WeatherForecast, bool>> expr2 = x => expr1.Invoke(x) && x.Id < 10;

        //AsExpandable() works on IQueryable<T>
        //Expand() works on Expression<TDelegate>

        var query1 = _context.WeatherForecasts.AsExpandable().Where(expr2).ToQueryString();
        var query2 = _context.WeatherForecasts.Where(expr2.Expand()).ToQueryString();

        expr1 = expr1.And(x => x.Id < 10);
        var query3 = _context.WeatherForecasts.Where(expr1).ToQueryString();

        return Ok($"{query1} \n\n {query2} \n\n {query3}");

    }
}
