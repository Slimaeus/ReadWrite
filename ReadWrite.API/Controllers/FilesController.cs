using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using ReadWrite.API.Services;

namespace ReadWrite.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly ExcelService _excelService;

    public FilesController(ExcelService excelService)
    {
        _excelService = excelService;
    }
    [HttpPost]
    public IActionResult PostExcelFile(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheet(1);

        List<Person> people = _excelService.ReadExcelFileWithColumnNames<Person>(stream, null);

        return Ok(people);
    }

    class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
