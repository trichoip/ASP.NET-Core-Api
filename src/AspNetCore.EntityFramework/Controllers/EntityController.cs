using AspNetCore.EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.EntityFramework.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly DataContext context;
        public EntityController(DataContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult CharactersList()
        {
            //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //context.Entry(entity).State = EntityState.Detached;

            //ontext.Attach(entity); là đưa entity vào dbcontext để quản lý
            //context.Entry(entity).State = EntityState.Modified; là đánh dấu entity là modified
            //context.Entry(entity).State = EntityState.Added; là đánh dấu entity là added
            //context.Entry(entity).State = EntityState.Deleted; là đánh dấu entity là deleted
            //context.Entry(entity).State = EntityState.Unchanged; là đánh dấu entity là unchanged
            //context.Entry(entity).State = EntityState.Detached; là đánh dấu entity là detached

            // để update hay remove entity thì entity phải được dbcontext quản lý , nếu không thì phải attach entity vào dbcontext
            // vì hàm update và remove là nó Attach entity vào dbcontext rùi đánh dấu entity là modified hoặc deleted,
            // mà nếu eniity chưa được quản lý bởi dbcontext có id trùng với entity được quản lý bởi dbcontext thì sẽ bị lỗi
            // nếu call hàm -> DTO GetById(DTO dto) mà trong hàm GetById có call hàm context.find, context.firstordefault,..... sau đó return về dto
            // rùi  map dto sang entity rùi update hay remove entity đó thì sẽ bị lỗi vì entity đó có cùng id với entity được quản lý trong dbcontext

            // để fix lỗi thì áp dụng AsNoTracking - có 3 cách
            // 1. khi dùng context để lấy entity thì thêm AsNoTracking() vào sau câu lệnh lấy entity đẻ entity đó không được quản lý bởi dbcontext
            // 2. dùng cấu lênh này trước khi get enity -> context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            // 3. đùng .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) sau .UseSqlServer -> nó ấp dụng AsNoTracking cho tất cả các câu lệnh lấy entity

            return Ok(context.Characters
                        // dùng AutoInclude trong ModelBuilder thì không cần Include ở đây
                        // xem cấu hình AutoInclude trong DataContext.cs
                        //.Include(c => c.Weapons)
                        //.Include(c => c.Backpack)
                        //.Include(c => c.Factions)
                        .ToList());
        }

    }
}
