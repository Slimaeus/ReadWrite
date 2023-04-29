using Microsoft.AspNetCore.Mvc;
using ReadWrite.API.Services;
using ReadWrite.Web.Models;

namespace ReadWrite.Web.Controllers;
public partial class PeopleController : Controller
{
    private static readonly List<Person> _people = new List<Person>();
    private readonly IExcelService _excelService;

    public PeopleController(IExcelService excelService)
    {
        _excelService = excelService;
    }
    public IActionResult ImportPeople()
    {
        return View();
    }
    [HttpPost]
    public IActionResult ImportPeople(IFormFile excelFile)
    {
        if (excelFile == null || excelFile.Length == 0)
        {
            ViewBag.Error = "Please select a file to upload.";
            return View("ImportPeople");
        }

        if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.Error = "Please select an Excel file (.xlsx).";
            return View("ImportPeople");
        }

        var people = _excelService.ReadExcelFileWithColumnNames<Person>(excelFile.OpenReadStream(), null);
        // Do something with the imported people data, such as saving to a database
        _people.AddRange(people);

        ViewBag.Success = $"Successfully imported {people.Count} people.";
        return RedirectToAction("Index");
    }
    public IActionResult Index()
    {
        return View(_people);
    }
}
