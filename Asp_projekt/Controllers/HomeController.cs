using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;
using Asp_projekt.Models;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(
        AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    [HttpGet("/")]
    public IActionResult Index()
    {
        var studenti = _db.Studenti
            .AsNoTracking()
            .Include(s => s.DaneOcjene)
            .ToList();

        var kolegiji = _db.Kolegiji
            .AsNoTracking()
            .Include(k => k.Profesori)
            .Include(k => k.Studenti)
            .ToList();

        var profesori = _db.Profesori
            .AsNoTracking()
            .Include(p => p.Ocjene)
            .Include(p => p.Kolegiji)
            .ToList();

        var ocjene = _db.Ocjene
            .AsNoTracking()
            .Include(o => o.Student)
            .Include(o => o.Kolegij)
            .ToList();

        var fakulteti = _db.Fakulteti
            .AsNoTracking()
            .Include(f => f.Profesori)
            .Include(f => f.Studenti)
            .Include(f => f.Kolegiji)
            .ToList();

        var izvjestaji = _db.Izvjestaji
            .AsNoTracking()
            .Include(i => i.Profesor)
            .ToList();

        var randomFakultet = PickRandom(fakulteti);

        var model = new HomeDashboardViewModel
        {
            Student = PickRandom(studenti),
            Kolegij = PickRandom(kolegiji),
            Profesor = PickRandom(profesori),
            Ocjena = PickRandom(ocjene),
            Fakultet = randomFakultet,
            FakultetId = randomFakultet?.Id,
            Izvjestaj = PickRandom(izvjestaji)
        };

        return View(model);
    }

    [HttpGet("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet("Error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static T? PickRandom<T>(List<T> items) where T : class
    {
        if (items.Count == 0)
        {
            return null;
        }

        return items[Random.Shared.Next(items.Count)];
    }
}
