using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class FakultetController : Controller
{
    private readonly AppDbContext _db;

    public FakultetController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var fakulteti = _db.Fakulteti
            .AsNoTracking()
            .Include(f => f.Profesori)
            .Include(f => f.Studenti)
            .Include(f => f.Kolegiji)
            .ToList();

        return View(fakulteti);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var fakultet = _db.Fakulteti
            .AsNoTracking()
            .Include(f => f.Profesori)
            .Include(f => f.Studenti)
            .Include(f => f.Kolegiji)
            .FirstOrDefault(f => f.Id == id);
        if (fakultet is null)
        {
            return NotFound();
        }

        return View(fakultet);
    }
}
