using AspNetCore.Excel.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace AspNetCore.Excel.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ExcelController : ControllerBase
	{
		string path = @"wwwroot\read.xlsx";

		[HttpGet]
		public IActionResult Read()
		{
			List<Foo> foos = new List<Foo>();
			ExcelPackage.LicenseContext = LicenseContext.Commercial;
			using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
			{

				ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
				var noOfCol = worksheet.Dimension.End.Column;
				var noOfRow = worksheet.Dimension.End.Row;

				var noOfStartRow = worksheet.Dimension.Start.Row + 1;
				var noOfStartCol = worksheet.Dimension.Start.Column;

				while (string.IsNullOrEmpty(worksheet.Cells[noOfStartRow, noOfStartCol].Value?.ToString()) == false)
				{
					foos.Add(new Foo
					{
						Id = Convert.ToInt32(worksheet.Cells[noOfStartRow, noOfStartCol].Value?.ToString()),
						Name = worksheet.Cells[noOfStartRow, noOfStartCol + 1].Value?.ToString(),
						Description = worksheet.Cells[noOfStartRow, noOfStartCol + 2].Value?.ToString()
					});
					noOfStartRow++;
				}
			}

			return Ok(foos);
		}

		[HttpPost]
		public async Task<IActionResult> Write(List<Foo> foos)
		{
			ExcelPackage.LicenseContext = LicenseContext.Commercial;
			using (var package = new ExcelPackage(new FileInfo(path)))
			{
				var worksheet = package.Workbook.Worksheets[0];
				worksheet.Cells.Clear();

				var range = worksheet.Cells["A1"].LoadFromCollection(foos, true);
				range.AutoFitColumns();
				await package.SaveAsync();
			}
			return Ok();
		}
	}
}

