using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;
using Asp_projekt.Models;

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
        var profesori = BuildProfesorQuery(null).ToList();

        return View(profesori);
    }

    [HttpGet("Search")]
    public IActionResult Search(string? q)
    {
        var profesori = BuildProfesorQuery(q).ToList();
        return PartialView("_ProfesorCards", profesori);
    }

    [HttpGet("Autocomplete")]
    public IActionResult Autocomplete(string? q)
    {
        var query = _db.Profesori
            .AsNoTracking()
            .Include(p => p.Fakultet)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(p =>
                p.Ime.Contains(term) ||
                p.Prezime.Contains(term) ||
                p.Katedra.Contains(term) ||
                ((p.Ime + " " + p.Prezime).Contains(term)));
        }

        var suggestions = query
            .OrderBy(p => p.Prezime)
            .ThenBy(p => p.Ime)
            .Take(8)
            .Select(p => new
            {
                value = p.Id,
                label = p.Ime + " " + p.Prezime,
                meta = string.IsNullOrWhiteSpace(p.Katedra)
                    ? (p.Fakultet != null ? p.Fakultet.Naziv : string.Empty)
                    : (p.Katedra + (p.Fakultet != null ? " · " + p.Fakultet.Naziv : string.Empty))
            })
            .ToList();

        return Json(suggestions);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var model = new ProfesorCreateViewModel();
        PopulateProfesorCreateOptions(model);
        return View(model);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProfesorCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateProfesorCreateOptions(model);
            return View(model);
        }

        if (model.FakultetId.HasValue)
        {
            var fakultetExists = _db.Fakulteti.Any(f => f.Id == model.FakultetId.Value);
            if (!fakultetExists)
            {
                ModelState.AddModelError(nameof(model.FakultetId), "Odabrani fakultet ne postoji.");
                PopulateProfesorCreateOptions(model);
                return View(model);
            }
        }

        var profesor = new Profesor(
            id: 0,
            ime: model.Ime.Trim(),
            prezime: model.Prezime.Trim(),
            katedra: model.Katedra.Trim())
        {
            FakultetId = model.FakultetId
        };

        _db.Profesori.Add(profesor);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var profesor = _db.Profesori
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
        if (profesor is null)
        {
            return NotFound();
        }

        var model = new ProfesorEditViewModel
        {
            Id = profesor.Id,
            Ime = profesor.Ime,
            Prezime = profesor.Prezime,
            Katedra = profesor.Katedra,
            FakultetId = profesor.FakultetId
        };
        PopulateProfesorEditOptions(model);

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, ProfesorEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            PopulateProfesorEditOptions(model);
            return View(model);
        }

        if (model.FakultetId.HasValue)
        {
            var fakultetExists = _db.Fakulteti.Any(f => f.Id == model.FakultetId.Value);
            if (!fakultetExists)
            {
                ModelState.AddModelError(nameof(model.FakultetId), "Odabrani fakultet ne postoji.");
                PopulateProfesorEditOptions(model);
                return View(model);
            }
        }

        var profesor = _db.Profesori.FirstOrDefault(p => p.Id == id);
        if (profesor is null)
        {
            return NotFound();
        }

        profesor.Ime = model.Ime.Trim();
        profesor.Prezime = model.Prezime.Trim();
        profesor.Katedra = model.Katedra.Trim();
        profesor.FakultetId = model.FakultetId;

        _db.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = profesor.Id });
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

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var profesor = _db.Profesori.FirstOrDefault(p => p.Id == id);
        if (profesor is null)
        {
            return NotFound();
        }

        _db.Profesori.Remove(profesor);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void PopulateProfesorCreateOptions(ProfesorCreateViewModel model)
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

    private void PopulateProfesorEditOptions(ProfesorEditViewModel model)
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

    private IQueryable<Profesor> BuildProfesorQuery(string? q)
    {
        var query = _db.Profesori
            .AsNoTracking()
            .Include(p => p.Ocjene)
            .Include(p => p.Kolegiji)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(p =>
                p.Ime.Contains(term) ||
                p.Prezime.Contains(term) ||
                p.Katedra.Contains(term) ||
                ((p.Ime + " " + p.Prezime).Contains(term)));
        }

        return query
            .OrderBy(p => p.Prezime)
            .ThenBy(p => p.Ime);
    }
}