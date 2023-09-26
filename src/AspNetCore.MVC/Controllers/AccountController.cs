using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net_core_empty_5._0.Models;
using asp.net_core_empty_5._0.Paging;
using asp.net_core_empty_5._0.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace asp.net_core_empty_5._0.Controllers
{

    public class AccountController : Controller
    {
        private readonly ETransportationSystemContext _context;
        private readonly IConfiguration Configuration;

        // BindProperty mac dinh la post neu them support get = true thi co the dung get
        [BindProperty(SupportsGet = true)] //data anotation
        public string Gender { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Number { get; set; }

        // neu khong co SupportsGet = true thi mac dinh la null, còn nếu có SupportsGet = true thì sẽ có giá trị empty
        [BindProperty(SupportsGet = true)]
        public Account AccountBinding { get; set; }

        [BindProperty]
        public AccountVM AccountVM { get; set; }

        public IList<AccountVM> AccountVMs { get; set; }

        public IList<Account> Accounts { get; set; }

        public IList<Car> Cars { get; set; }

        // paging - sort - filter

        //[BindProperty(SupportsGet = true)]
        //public string SearchString { get; set; }

        //[BindProperty(SupportsGet = true)]
        //public string SortField { get; set; }

        public string NameSort { get; set; }
        public string BirthDateSort { get; set; }
        public string IdSort { get; set; }
        public string CurrentFilterSearchString { get; set; }
        public string CurrentSortField { get; set; }
        public PaginatedList<Account> AccountPaging { get; set; }

        public AccountController(ETransportationSystemContext context, IConfiguration _configuration)
        {
            Configuration = _configuration;
            _context = context;

        }

        public IActionResult Index(Account accountpro)
        {

            ViewBag.Number = Number;
            ViewBag.Ava = accountpro.Avatar;
            ViewBag.AccountBinding = AccountBinding.Avatar;
            ViewData["Tilte"] = "Welcome to dotnet";
            ViewBag.Trichoip = "Hello trichoip pro";

            //TempData chỉ truyền data qua 1 request nếu muốn giữ qua nhiều lần request thì phải sử dụng TempData.Keep
            TempData["Message"] = "Hello world!";
            TempData.Peek("Message");
            TempData.Keep("Message");

            List<Account> _accounts;
            using (var context = new ETransportationSystemContext())
            {
                _accounts = context.Accounts.ToList();
            }
            ViewBag.Account = _accounts[0];

            return View();
        }

        // paging - sort - filter
        public async Task<IActionResult> List(string SortField, string SearchString, int? pageIndex)
        {
            // model AccountController
            //Accounts = _context.Accounts.ToList();
            Cars = _context.Cars.ToList();

            // paging - sort - filter
            NameSort = String.IsNullOrEmpty(SortField) ? "name_desc" : "";
            BirthDateSort = SortField == "birthDate" ? "birthDate_desc" : "birthDate";
            IdSort = SortField == "id" ? "id_desc" : "id";

            CurrentSortField = SortField;
            CurrentFilterSearchString = SearchString;

            // câu lệnh AccountsDBS = _context.Accounts chỉ thiết lập một tham chiếu đến tập hợp dữ liệu,
            // không thực hiện truy vấn trên cơ sở dữ liệu.
            // Truy vấn sẽ được thực hiện khi bạn thực hiện các hoạt động yêu cầu dữ liệu từ AccountsDBS(ToListAsync).
            var AccountsDBS = _context.Accounts;

            // Khi một IQueryable được tạo hoặc sửa đổi, không có query  nào được gửi đến cơ sở dữ liệu.
            // query  không được thực thi cho đến khi IQueryable đối tượng được chuyển đổi thành collection(ToListAsync).
            IQueryable<Account> AccountsIQ = from s in _context.Accounts select s;

            // search
            if (!String.IsNullOrEmpty(SearchString))
            {
                AccountsIQ = AccountsIQ.Where(s => s.Name.Contains(SearchString));
            }

            //sort
            switch (SortField)
            {
                case "name_desc":
                    AccountsIQ = AccountsIQ.OrderByDescending(s => s.Name);
                    break;
                case "birthDate":
                    AccountsIQ = AccountsIQ.OrderBy(s => s.BirthDate);
                    break;
                case "birthDate_desc":
                    AccountsIQ = AccountsIQ.OrderByDescending(s => s.BirthDate);
                    break;
                case "id":
                    AccountsIQ = AccountsIQ.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    AccountsIQ = AccountsIQ.OrderByDescending(s => s.Id);
                    break;
                default:
                    AccountsIQ = AccountsIQ.OrderBy(s => s.Name);
                    break;
            }
            // Do đó, IQueryable kết quả code  trong một query  duy nhất không được thực hiện cho đến câu lệnh sau:
            Accounts = await AccountsIQ.AsNoTracking().ToListAsync();

            // paging
            var pageSize = Configuration.GetValue("PageSize", 4);
            AccountPaging = await PaginatedList<Account>.CreateAsync(AccountsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            return View(this);
        }

        public IActionResult Details(long id)
        {
            // FindAsync Find là tìm theo primary key
            // SingleOrDefaultAsync SingleOrDefault Đưa ra một exception nếu có nhiều thực thể thỏa mãn bộ lọc truy vấn

            Account _account;
            using (var context = new ETransportationSystemContext())
            {
                // AsNoTracking() để chỉ định rằng các thực thể trả về từ truy vấn sẽ không được theo dõi (tracking).
                // Khi sử dụng phương thức này, Entity Framework Core sẽ không theo dõi sự thay đổi của các thực thể được trả về và không tự động cập nhật cơ sở dữ liệu khi lưu các thay đổi.
                // Khi sử dụng .AsNoTracking(), truy vấn trả về các thực thể sẽ có hiệu suất tốt hơn vì không cần theo dõi các thay đổi và xử lý việc cập nhật
                //Lưu ý rằng khi sử dụng .AsNoTracking(), các thực thể không được theo dõi không được gắn kết với DbContext.
                // Điều này có nghĩa là nếu bạn muốn thực hiện các thay đổi trên các thực thể và lưu chúng vào cơ sở dữ liệu,
                // bạn cần gắn kết các thực thể đó với DbContext bằng cách sử dụng phương thức Attach() hoặc Update().
                _account = context.Accounts.AsNoTracking()
                                    .Include(a => a.DrivingLicense).AsNoTracking()
                                    .Include(c => c.Cars)
                                        .ThenInclude(c => c.Model)
                                        .ThenInclude(m => m.Brand).AsNoTracking()
                                    .Include(c => c.Cars)
                                        .ThenInclude(c => c.CarImages).AsNoTracking()
                                    .FirstOrDefault(c => c.Id == id);

                _account = context.Accounts
                   .Where(a => a.Id == id)
                   .Select(a => new Account
                   {
                       Id = a.Id,
                       Name = a.Name,
                       BirthDate = a.BirthDate,
                       Email = a.Email,
                       DrivingLicense = a.DrivingLicense,
                       Cars = a.Cars.Select(c => new Car
                       {
                           Id = c.Id,
                           Model = new Model
                           {
                               Id = c.Model.Id,
                               Name = c.Model.Name,
                               Brand = new Brand
                               {
                                   Id = c.Model.Brand.Id,
                                   Name = c.Model.Brand.Name
                               }
                           },
                           CarImages = c.CarImages.Select(ig => new CarImage()
                           {
                               Id = ig.Id,
                               Image = ig.Image
                           }).ToList()
                       }).ToList()
                   })
                   .FirstOrDefault();

            }
            return View(_account);
        }

        public IActionResult RedirectTest()
        {
            return Redirect(Url.Action(nameof(FeatureController.Index1), "Feature"));
            return Redirect(nameof(Index));

            return View("../Validation/Index");// khac controller
            return View("Views/Validation/Index.cshtml");// khac controller
            return Redirect("/car/details?id=5&name=John&age=30");

            // http://localhost:5000/car/details/5?name=John&age=30
            var routeValues = new RouteValueDictionary(new { id = 5, name = "John", age = 30 });
            return RedirectToAction("details", routeValues);
            return RedirectToAction("details", new { id = 5 });

            return RedirectToAction("index", "feature", routeValues);
            return RedirectToAction("index", "feature");
        }

        public string ParamAction(string Name, int Age)
        {
            //string Name = Request.Query["Name"];
            //int Age = Int32.Parse(Request.Query["Age"]);

            return $"Name {Name} Age {Age}";

        }

        public string ParamContructor()
        {
            return $"gender: {Gender} number: {Number}";
        }

        public async Task<string> UpdateAsync()
        {
            // SaveChangesAsync SaveChanges
            // Added: Thực thể chưa tồn tại trong cơ sở dữ liệu. Phương SaveChangespháp đưa ra một INSERTtuyên bố.
            // Unchanged: Không cần lưu thay đổi với thực thể này. Một thực thể có trạng thái này khi nó được đọc từ cơ sở dữ liệu.
            // Modified: Một số hoặc tất cả các giá trị thuộc tính của thực thể đã được sửa đổi. Phương SaveChangespháp đưa ra một UPDATE tuyên bố.
            // Deleted: Thực thể đã được đánh dấu để xóa. Phương SaveChangespháp đưa ra một DELETEtuyên bố.
            // Detached: Thực thể không được theo dõi bởi bối cảnh cơ sở dữ liệu.
            var accountToUpdate = await _context.Accounts.FindAsync(AccountBinding.Id);
            if (await TryUpdateModelAsync<Account>(accountToUpdate, "AccountBinding", s => s.Id, s => s.Balance))
            {
                //_context.Accounts.Add(accountToUpdate);
                //await _context.SaveChangesAsync();
                return "oke";
            }

            //mapper
            var entry = _context.Add(new Account());
            // Phương thức SetValues được sử dụng để cập nhật các thuộc tính của thực thể từ một đối tượng khác,
            // trong trường hợp này là AccountVM
            // Nếu các thuộc tính của đối tượng AccountVM có tên và kiểu dữ liệu tương ứng với các thuộc tính của đối tượng Account,
            // phương thức SetValues sẽ sao chép các giá trị từ AccountVM vào CurrentValues của entry,
            // ghi đè lên các giá trị hiện tại của thực thể Account.
            entry.CurrentValues.SetValues(AccountVM);
            //_context.Attach(entry);
            //await _context.SaveChangesAsync();
            return "oke";
        }

        public async Task<IActionResult> ViewModelMapperAsync()
        {
            IQueryable<AccountVM> data = from account in _context.Accounts
                                         group account by account.Id into dateGroup
                                         select new AccountVM()
                                         {
                                             Id = dateGroup.Key,
                                             Balance = dateGroup.Count()
                                         };

            AccountVMs = await data.AsNoTracking().ToListAsync();
            return View(this);
        }

        //[HttpPost]
        //[HttpGet("{id}")]
        [HttpPost, ActionName("Delete")]  // POST: Movies/Delete/5
        //[HttpGet]
        //[HttpGet, ActionName("Delete")]
        // mặc định là HttpGet , khi cấu hình [HttpPost] thì phải truy cập thông qua method post mới  có thể truy cập được
        // khi đã cấu hình ActionName thì ActionName không còn là mặc định là tên function nữa mà theo tên đẫ cấu hình
        public string DeleteTrichoip()
        {
            return "oke";
        }

    }
}
