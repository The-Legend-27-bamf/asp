using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;
using Asp_projekt.Models;

namespace Asp_projekt.Controllers;

[Route("[controller]")]
public class OcjenaController : Controller
{
    private readonly AppDbContext _db;

    public OcjenaController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var ocjene = _db.Ocjene
            .AsNoTracking()
            .Include(o => o.Student)
            .Include(o => o.Kolegij)
            .ToList();

        return View(ocjene);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var model = new OcjenaCreateViewModel
        {
            Profesori = _db.Profesori
                .AsNoTracking()
                .Include(p => p.Fakultet)
                .OrderBy(p => p.Prezime)
                .ThenBy(p => p.Ime)
                .Select(p => new OcjenaCreateViewModel.ProfesorOption
                {
                    Id = p.Id,
                    Naziv = $"{p.Ime} {p.Prezime}",
                    Katedra = p.Katedra,
                    FakultetNaziv = p.Fakultet != null ? p.Fakultet.Naziv : null
                })
                .ToList(),
            Studenti = _db.Studenti
                .AsNoTracking()
                .Include(s => s.Fakultet)
                .OrderBy(s => s.Prezime)
                .ThenBy(s => s.Ime)
                .Select(s => new OcjenaCreateViewModel.StudentOption
                {
                    Id = s.Id,
                    Naziv = $"{s.Ime} {s.Prezime}",
                    DatumUpisa = s.DatumUpisa,
                    FakultetNaziv = s.Fakultet != null ? s.Fakultet.Naziv : null
                })
                .ToList(),
            Kolegiji = _db.Kolegiji
                .AsNoTracking()
                .Include(k => k.Fakultet)
                .OrderBy(k => k.Naziv)
                .Select(k => new OcjenaCreateViewModel.KolegijOption
                {
                    Id = k.Id,
                    Naziv = k.Naziv,
                    ECTS = k.ECTS,
                    FakultetNaziv = k.Fakultet != null ? k.Fakultet.Naziv : null
                })
                .ToList()
        };

        return View(model);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(OcjenaCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Profesori = _db.Profesori
                .AsNoTracking()
                .Include(p => p.Fakultet)
                .OrderBy(p => p.Prezime)
                .ThenBy(p => p.Ime)
                .Select(p => new OcjenaCreateViewModel.ProfesorOption
                {
                    Id = p.Id,
                    Naziv = $"{p.Ime} {p.Prezime}",
                    Katedra = p.Katedra,
                    FakultetNaziv = p.Fakultet != null ? p.Fakultet.Naziv : null
                })
                .ToList();
            model.Studenti = _db.Studenti
                .AsNoTracking()
                .Include(s => s.Fakultet)
                .OrderBy(s => s.Prezime)
                .ThenBy(s => s.Ime)
                .Select(s => new OcjenaCreateViewModel.StudentOption
                {
                    Id = s.Id,
                    Naziv = $"{s.Ime} {s.Prezime}",
                    DatumUpisa = s.DatumUpisa,
                    FakultetNaziv = s.Fakultet != null ? s.Fakultet.Naziv : null
                })
                .ToList();
            model.Kolegiji = _db.Kolegiji
                .AsNoTracking()
                .Include(k => k.Fakultet)
                .OrderBy(k => k.Naziv)
                .Select(k => new OcjenaCreateViewModel.KolegijOption
                {
                    Id = k.Id,
                    Naziv = k.Naziv,
                    ECTS = k.ECTS,
                    FakultetNaziv = k.Fakultet != null ? k.Fakultet.Naziv : null
                })
                .ToList();

            return View(model);
        }

        var profesor = _db.Profesori.FirstOrDefault(p => p.Id == model.ProfesorId);
        if (profesor is null)
        {
            return NotFound();
        }

        var student = _db.Studenti.FirstOrDefault(s => s.Id == model.StudentId);
        if (student is null)
        {
            return NotFound();
        }

        var kolegij = _db.Kolegiji.FirstOrDefault(k => k.Id == model.KolegijId);
        if (kolegij is null)
        {
            return NotFound();
        }

        var ocjena = new Ocjena(
            id: 0,
            vrijednost: model.Vrijednost,
            komentar: model.Komentar,
            datumOcjene: model.DatumOcjene,
            tip: model.Tip,
            profesor: profesor,
            student: student,
            kolegij: kolegij);

        _db.Ocjene.Add(ocjena);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var ocjena = _db.Ocjene
            .AsNoTracking()
            .Include(o => o.Profesor)
            .Include(o => o.Student)
            .Include(o => o.Kolegij)
            .FirstOrDefault(o => o.Id == id);
        if (ocjena is null)
        {
            return NotFound();
        }

        return View(ocjena);
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var ocjena = _db.Ocjene.FirstOrDefault(o => o.Id == id);
        if (ocjena is null)
        {
            return NotFound();
        }

        _db.Ocjene.Remove(ocjena);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
