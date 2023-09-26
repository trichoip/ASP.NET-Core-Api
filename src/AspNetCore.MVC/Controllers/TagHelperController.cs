using asp.net_core_empty_5._0.Models;
using asp.net_core_empty_5._0.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace asp.net_core_empty_5._0.Controllers
{
    public class TagHelperController : Controller
    {
        public IList<Movie> Movies { get; set; } = new Collection<Movie>
        {
            new Movie { ID = 1, Title = "The Shawshank Redemption 1" ,Address = new AddressTest{ nameAddress = "address1"} },
            new Movie { ID = 2, Title = "The Shawshank Redemption 2" ,Address = new AddressTest{ nameAddress = "address2"} },
            new Movie { ID = 3, Title = "The Shawshank Redemption 3" ,Address = new AddressTest{ nameAddress = "address3"} },
            new Movie { ID = 4, Title = "The Shawshank Redemption 4" ,Address = new AddressTest{ nameAddress = "address4"} },
        };

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public Movie Movie { get; set; }
        public IActionResult Tag(Account accountpro)
        {
            //_context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            //(_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();

            //selectListTag
            Movie = new Movie { ID = 3, isActive = false, EnumCountry = CountryEnum.Canada };
            ViewData["SelectListTag"] = new SelectList(Movies, "ID", "Address.nameAddress", "2", "Address.nameAddress");
            ViewData["SelectListTag2"] = new SelectList(Movies, "ID", "Title");

            var items = new List<SelectListItem>
                {
                     new SelectListItem { Value = "1", Text = "Option 1" },
                      new SelectListItem { Value = "2", Text = "Option 2" },
                     new SelectListItem { Value = "3", Text = "Option 3" }
                };
            var selectList = new SelectList(items, "Value", "Text", "3");
            ViewBag.SelectList = selectList;
            // selectListTag end

            if (!ModelState.IsValid)
            {
                return View(this);
            }
            return View(this);
            return NotFound();
        }

    }

}
