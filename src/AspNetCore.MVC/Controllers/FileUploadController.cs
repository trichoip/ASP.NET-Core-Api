using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace asp.net_core_empty_5._0.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileUploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [Required]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif")]   // not working
        [Display(Name = "Choose file(s) to Upload")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        private string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".gif" };

        public async Task<IActionResult> Upload()
        {
            if (HttpContext.Request.Method == "GET")
            {
                return View(this);
            }

            if (FileUploads != null)
            {
                foreach (var FileUpload in FileUploads)
                {
                    var ext = Path.GetExtension(FileUpload.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("FileUploads", "Invalid file extension.");
                        return View(this);
                    }
                    var file = Path.Combine(_webHostEnvironment.WebRootPath, "images", FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                        ModelState.AddModelError(string.Empty, "Upload Succesfully");
                    }
                }
            }
            return View(this);
        }
    }
}
