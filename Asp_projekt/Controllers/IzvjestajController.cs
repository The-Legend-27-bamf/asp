using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;
using Asp_projekt.Models;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class IzvjestajController : Controller
{
    private const string ReadOnlyReason = "Izvjestaj is read-only because it is derived data generated from existing Ocjene and linked Profesor metrics.";
    private readonly AppDbContext _db;

    public IzvjestajController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var izvjestaji = BuildIzvjestajQuery(null).ToList();

        return View(izvjestaji);
    }

    [HttpGet("Search")]
    public IActionResult Search(string? q)
    {
        var izvjestaji = BuildIzvjestajQuery(q).ToList();
        return PartialView("_IzvjestajCards", izvjestaji);
    }

    [HttpGet("Create")]
    [HttpGet("Edit/{id:int}")]
    public IActionResult ReadOnlyGuardGet()
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed, ReadOnlyReason);
    }

    [HttpPost("Create")]
    [HttpPost("Edit/{id:int}")]
    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult ReadOnlyGuardPost()
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed, ReadOnlyReason);
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

    private IQueryable<Izvjestaj> BuildIzvjestajQuery(string? q)
    {
        var query = _db.Izvjestaji
            .AsNoTracking()
            .Include(i => i.Profesor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(i =>
                i.Profesor.Ime.Contains(term) ||
                i.Profesor.Prezime.Contains(term) ||
                ((i.Profesor.Ime + " " + i.Profesor.Prezime).Contains(term)) ||
                i.Profesor.Katedra.Contains(term));
        }

        return query.OrderByDescending(i => i.DatumGeneriranja);
    }
}
