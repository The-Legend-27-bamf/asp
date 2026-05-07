using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class KolegijController : Controller
{
    private readonly AppDbContext _db;

    public KolegijController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var kolegiji = _db.Kolegiji
            .AsNoTracking()
            .Include(k => k.Profesori)
            .Include(k => k.Studenti)
            .ToList();

        return View(kolegiji);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var kolegij = _db.Kolegiji
            .AsNoTracking()
            .Include(k => k.Profesori)
            .Include(k => k.Studenti)
            .FirstOrDefault(k => k.Id == id);
        if (kolegij is null)
        {
            return NotFound();
        }

        return View(kolegij);
    }
}