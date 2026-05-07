using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class ProfesorController : Controller
{
    private readonly AppDbContext _db;

    public ProfesorController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var profesori = _db.Profesori
            .AsNoTracking()
            .Include(p => p.Ocjene)
            .Include(p => p.Kolegiji)
            .ToList();

        return View(profesori);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var profesor = _db.Profesori
            .AsNoTracking()
            .Include(p => p.Ocjene)
                .ThenInclude(o => o.Student)
            .Include(p => p.Ocjene)
                .ThenInclude(o => o.Kolegij)
            .Include(p => p.Kolegiji)
            .FirstOrDefault(p => p.Id == id);
        if (profesor is null)
        {
            return NotFound();
        }

        return View(profesor);
    }
}