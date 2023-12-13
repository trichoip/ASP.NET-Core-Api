using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCore.Api.Cache.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MemoryCacheController : ControllerBase
{
    private readonly IMemoryCache cache;

    public MemoryCacheController(IMemoryCache cache)
    {
        this.cache = cache;
    }

    [HttpGet]
    public IActionResult InputCache(string name)
    {
        var people = new Faker<People>()
            .RuleFor(p => p.id, f => f.Random.Int(1, 10000))
            .RuleFor(p => p.name, f => f.Name.FullName())
            .RuleFor(p => p.age, f => f.Random.Number(18, 60))
            .RuleFor(p => p.address, f => f.Address.FullAddress())
            .RuleFor(p => p.city, f => f.Address.City())
            .Generate(3);

        var options = new MemoryCacheEntryOptions()
        {
            // thời gian tuyệt đối nghĩa là item được cache sẽ bị gỡ bỏ tại một thời điểm ngày và giờ rõ ràng
            // này không có gia hạn, cứ cùng time là xóa cache
            AbsoluteExpiration = DateTime.Now.AddSeconds(60),

            // thời gian tương đối ở đây là cache sẽ được gỡ bỏ sau một khoảng thời gian nhất định không truy cập đến
            // còn nếu đang truy cập thì sẽ được gia hạn thêm
            SlidingExpiration = TimeSpan.FromSeconds(50),
        };
        options.RegisterPostEvictionCallback(MyCallback, this);

        //Dựa vào các mức độ ưu tiên trên thì cache sẽ bị xóa từ item có độ ưu tiên thấp đến cao,
        //riêng NeverRemove thì sẽ không bị xóa với bất kỳ lý do gì.
        // ví dụ: khi cache đầy thì sẽ xóa item có độ ưu tiên thấp nhất
        // cache a là Low và cache b là Normal thì khi cache đầy thì sẽ xóa cache a trước
        options.Priority = CacheItemPriority.Normal;

        cache.Set<IEnumerable<People>>(name, people, options);

        return Ok(people);
    }

    [HttpGet]
    public IActionResult CheckMessages(string name)
    {
        if (cache.Get<string>("callbackMessage") is not { } data) return Ok("chua het han");

        return Ok(data);
    }

    [HttpGet]
    public IActionResult CheckCacheAndGet(string name)
    {
        if (cache.Get<IEnumerable<People>>(name) is not { } data) return NotFound();

        return Ok(data);
    }

    [HttpGet]
    public IActionResult CheckCacheAndGet2(string name)
    {
        if (!cache.TryGetValue(name, out object data)) return NotFound();

        if (!cache.TryGetValue(name, out var data2)) return NotFound();

        if (!cache.TryGetValue<IEnumerable<People>>(name, out var data3)) return NotFound();

        if (!cache.TryGetValue(name, out IEnumerable<People> data4)) return NotFound();

        return Ok(new { data, data2, data3, data4 });
    }

    [HttpGet]
    public async Task<IActionResult> GetOrCreate(string name)
    {

        var people = new Faker<People>()
             .RuleFor(p => p.id, f => f.Random.Int(1, 10000))
             .RuleFor(p => p.name, f => f.Name.FullName())
             .RuleFor(p => p.age, f => f.Random.Number(18, 60))
             .RuleFor(p => p.address, f => f.Address.FullAddress())
             .RuleFor(p => p.city, f => f.Address.City())
             .Generate(3);

        var a = await cache.GetOrCreateAsync(name, entry =>
           {
               //entry.SetSlidingExpiration(TimeSpan.FromSeconds(10));
               return Task.FromResult(people);
           });

        return Ok(a);
    }

    [HttpGet]
    // chỉ áp dụng cho HttpGet
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByHeader = "User-Agent")]
    public IActionResult InputCache2()
    {

        var cachedPeople = new Faker<People>()
              .RuleFor(p => p.id, f => f.Random.Int(1, 10000))
              .RuleFor(p => p.name, f => f.Name.FullName())
              .RuleFor(p => p.age, f => f.Random.Number(18, 60))
              .RuleFor(p => p.address, f => f.Address.FullAddress())
              .RuleFor(p => p.city, f => f.Address.City())
              .Generate(3);

        return Ok(cachedPeople);
    }

    private static void MyCallback(object key, object value, EvictionReason reason, object state)
    {
        var message = $"Cache entry was removed : {reason}";

        ((MemoryCacheController)state).cache.Set("callbackMessage", message);
    }
}

public class People
{
    public int id { get; set; }
    public string name { get; set; }
    public int age { get; set; }
    public string address { get; set; }
    public string city { get; set; }
}