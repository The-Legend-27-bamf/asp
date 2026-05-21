using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;
using Asp_projekt.Models;

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
        var kolegiji = BuildKolegijQuery(null).ToList();

        return View(kolegiji);
    }

    [HttpGet("Search")]
    public IActionResult Search(string? q)
    {
        var kolegiji = BuildKolegijQuery(q).ToList();
        return PartialView("_KolegijCards", kolegiji);
    }

    [HttpGet("Autocomplete")]
    public IActionResult Autocomplete(string? q)
    {
        var query = _db.Kolegiji
            .AsNoTracking()
            .Include(k => k.Fakultet)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(k =>
                k.Naziv.Contains(term));
        }

        var suggestions = query
            .OrderBy(k => k.Naziv)
            .Take(8)
            .Select(k => new
            {
                value = k.Id,
                label = k.Naziv,
                meta = "ECTS " + k.ECTS + (k.Fakultet != null ? " · " + k.Fakultet.Naziv : string.Empty)
            })
            .ToList();

        return Json(suggestions);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var model = new KolegijCreateViewModel();
        PopulateKolegijCreateOptions(model);
        return View(model);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(KolegijCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Unos kolegija nije uspio. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            PopulateKolegijCreateOptions(model);
            return View(model);
        }

        if (model.FakultetId.HasValue && !_db.Fakulteti.Any(f => f.Id == model.FakultetId.Value))
        {
            ModelState.AddModelError(nameof(model.FakultetId), "Odabrani fakultet ne postoji.");
            ViewData["ToastMessage"] = "Unos kolegija nije uspio. Odabrani fakultet ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateKolegijCreateOptions(model);
            return View(model);
        }

        var kolegij = new Kolegij(
            id: 0,
            naziv: model.Naziv.Trim(),
            ects: model.ECTS)
        {
            FakultetId = model.FakultetId
        };

        _db.Kolegiji.Add(kolegij);
        _db.SaveChanges();

        TempData["ToastMessage"] = "Kolegij je uspjesno kreiran.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var kolegij = _db.Kolegiji
            .AsNoTracking()
            .FirstOrDefault(k => k.Id == id);
        if (kolegij is null)
        {
            return NotFound();
        }

        var model = new KolegijEditViewModel
        {
            Id = kolegij.Id,
            Naziv = kolegij.Naziv,
            ECTS = kolegij.ECTS,
            FakultetId = kolegij.FakultetId
        };
        PopulateKolegijEditOptions(model);

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, KolegijEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Azuriranje kolegija nije uspjelo. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            PopulateKolegijEditOptions(model);
            return View(model);
        }

        if (model.FakultetId.HasValue && !_db.Fakulteti.Any(f => f.Id == model.FakultetId.Value))
        {
            ModelState.AddModelError(nameof(model.FakultetId), "Odabrani fakultet ne postoji.");
            ViewData["ToastMessage"] = "Azuriranje kolegija nije uspjelo. Odabrani fakultet ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateKolegijEditOptions(model);
            return View(model);
        }

        var kolegij = _db.Kolegiji.FirstOrDefault(k => k.Id == id);
        if (kolegij is null)
        {
            return NotFound();
        }

        kolegij.Naziv = model.Naziv.Trim();
        kolegij.ECTS = model.ECTS;
        kolegij.FakultetId = model.FakultetId;

        _db.SaveChanges();

        TempData["ToastMessage"] = "Kolegij je uspjesno azuriran.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Details), new { id = kolegij.Id });
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

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var kolegij = _db.Kolegiji
            .Include(k => k.Studenti)
            .FirstOrDefault(k => k.Id == id);
        if (kolegij is null)
        {
            return NotFound();
        }

        foreach (var student in kolegij.Studenti)
        {
            student.KolegijId = null;
        }

        _db.Kolegiji.Remove(kolegij);
        _db.SaveChanges();

        TempData["ToastMessage"] = "Kolegij je uspjesno obrisan.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    private void PopulateKolegijCreateOptions(KolegijCreateViewModel model)
    {
        model.Fakulteti = _db.Fakulteti
            .AsNoTracking()
            .OrderBy(f => f.Naziv)
            .Select(f => new LookupOption
            {
                Id = f.Id,
                Naziv = f.Naziv
            })
            .ToList();
    }

    private void PopulateKolegijEditOptions(KolegijEditViewModel model)
    {
        model.Fakulteti = _db.Fakulteti
            .AsNoTracking()
            .OrderBy(f => f.Naziv)
            .Select(f => new LookupOption
            {
                Id = f.Id,
                Naziv = f.Naziv
            })
            .ToList();
    }

    private IQueryable<Kolegij> BuildKolegijQuery(string? q)
    {
        var query = _db.Kolegiji
            .AsNoTracking()
            .Include(k => k.Profesori)
            .Include(k => k.Studenti)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(k =>
                k.Naziv.Contains(term) ||
                k.Profesori.Any(p => p.Ime.Contains(term) || p.Prezime.Contains(term)) ||
                k.Studenti.Any(s => s.Ime.Contains(term) || s.Prezime.Contains(term)));
        }

        return query.OrderBy(k => k.Naziv);
    }
}