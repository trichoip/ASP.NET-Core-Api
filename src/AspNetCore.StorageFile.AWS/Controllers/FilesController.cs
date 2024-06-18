using AspNetCore.StorageFile.AWS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.StorageFile.AWS.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IStorageService _storageService;

    public FilesController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var uploadedFileName = await _storageService.UploadFileAsync(file);

        return Ok(new
        {
            FileName = uploadedFileName,
            Url = _storageService.GetObjectUrl(uploadedFileName)
        });
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
    {
        var bytes = await _storageService.DownloadFileAsync(fileName);
        var contentType = await _storageService.GetContentType(fileName);
        return File(bytes, contentType);
    }

    [HttpGet("{fileName}/presigned-url")]
    public async Task<IActionResult> GetPresignedUrl([FromRoute] string fileName)
    {
        var url = await _storageService.GetPresignedUrlAsync(fileName);
        return Ok(new { url });
    }
}