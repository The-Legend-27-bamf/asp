using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;
using Asp_projekt.Models;

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
        var fakulteti = BuildFakultetQuery(null).ToList();

        return View(fakulteti);
    }

    [HttpGet("Search")]
    public IActionResult Search(string? q)
    {
        var fakulteti = BuildFakultetQuery(q).ToList();
        return PartialView("_FakultetCards", fakulteti);
    }

    [HttpGet("Autocomplete")]
    public IActionResult Autocomplete(string? q)
    {
        var query = _db.Fakulteti
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(f => f.Naziv.Contains(term));
        }

        var suggestions = query
            .OrderBy(f => f.Naziv)
            .Take(8)
            .Select(f => new
            {
                value = f.Id,
                label = f.Naziv,
                meta = string.Empty
            })
            .ToList();

        return Json(suggestions);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new FakultetCreateViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(FakultetCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Unos fakulteta nije uspio. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            return View(model);
        }

        var exists = _db.Fakulteti.Any(f => f.Naziv == model.Naziv.Trim());
        if (exists)
        {
            ModelState.AddModelError(nameof(model.Naziv), "Fakultet s tim nazivom vec postoji.");
            ViewData["ToastMessage"] = "Unos fakulteta nije uspio. Fakultet vec postoji.";
            ViewData["ToastType"] = "warning";
            return View(model);
        }

        var fakultet = new Fakultet(model.Naziv.Trim());
        _db.Fakulteti.Add(fakultet);
        _db.SaveChanges();

        TempData["ToastMessage"] = "Fakultet je uspjesno kreiran.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var fakultet = _db.Fakulteti
            .AsNoTracking()
            .FirstOrDefault(f => f.Id == id);
        if (fakultet is null)
        {
            return NotFound();
        }

        var model = new FakultetEditViewModel
        {
            Id = fakultet.Id,
            Naziv = fakultet.Naziv
        };

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, FakultetEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Azuriranje fakulteta nije uspjelo. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            return View(model);
        }

        var fakultet = _db.Fakulteti.FirstOrDefault(f => f.Id == id);
        if (fakultet is null)
        {
            return NotFound();
        }

        var naziv = model.Naziv.Trim();
        var duplicate = _db.Fakulteti.Any(f => f.Id != id && f.Naziv == naziv);
        if (duplicate)
        {
            ModelState.AddModelError(nameof(model.Naziv), "Fakultet s tim nazivom vec postoji.");
            ViewData["ToastMessage"] = "Azuriranje fakulteta nije uspjelo. Fakultet vec postoji.";
            ViewData["ToastType"] = "warning";
            return View(model);
        }

        fakultet.Naziv = naziv;
        _db.SaveChanges();

        TempData["ToastMessage"] = "Fakultet je uspjesno azuriran.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Details), new { id = fakultet.Id });
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

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var fakultet = _db.Fakulteti
            .Include(f => f.Profesori)
            .Include(f => f.Studenti)
            .Include(f => f.Kolegiji)
            .FirstOrDefault(f => f.Id == id);
        if (fakultet is null)
        {
            return NotFound();
        }

        // Break optional relations first to avoid FK constraint issues.
        foreach (var profesor in fakultet.Profesori)
        {
            profesor.FakultetId = null;
        }

        foreach (var student in fakultet.Studenti)
        {
            student.FakultetId = null;
        }

        foreach (var kolegij in fakultet.Kolegiji)
        {
            kolegij.FakultetId = null;
        }

        _db.Fakulteti.Remove(fakultet);
        _db.SaveChanges();

        TempData["ToastMessage"] = "Fakultet je uspjesno obrisan.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    private IQueryable<Fakultet> BuildFakultetQuery(string? q)
    {
        var query = _db.Fakulteti
            .AsNoTracking()
            .Include(f => f.Profesori)
            .Include(f => f.Studenti)
            .Include(f => f.Kolegiji)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(f => f.Naziv.Contains(term));
        }

        return query.OrderBy(f => f.Naziv);
    }
}
