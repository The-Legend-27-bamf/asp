using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class IzvjestajController : Controller
{
    private readonly AppDbContext _db;

    public IzvjestajController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var izvjestaji = _db.Izvjestaji
            .AsNoTracking()
            .Include(i => i.Profesor)
            .ToList();

        return View(izvjestaji);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var izvjestaj = _db.Izvjestaji
            .AsNoTracking()
            .Include(i => i.Profesor)
            .FirstOrDefault(i => i.Id == id);
        if (izvjestaj is null)
        {
            return NotFound();
        }

        return View(izvjestaj);
    }
}
