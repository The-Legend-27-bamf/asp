using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class StudentController : Controller
{
    private readonly AppDbContext _db;

    public StudentController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var studenti = _db.Studenti
            .AsNoTracking()
            .Include(s => s.DaneOcjene)
            .ToList();

        return View(studenti);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var student = _db.Studenti
            .AsNoTracking()
            .Include(s => s.DaneOcjene)
                .ThenInclude(o => o.Kolegij)
            .Include(s => s.DaneOcjene)
                .ThenInclude(o => o.Profesor)
            .FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return NotFound();
        }

        return View(student);
    }
}