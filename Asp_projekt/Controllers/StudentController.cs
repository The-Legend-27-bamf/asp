using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_projekt.Data;
using Asp_projekt.Models;

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
        var studenti = BuildStudentQuery(null).ToList();

        return View(studenti);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var model = new StudentCreateViewModel();
        PopulateStudentCreateOptions(model);
        return View(model);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(StudentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Unos studenta nije uspio. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            PopulateStudentCreateOptions(model);
            return View(model);
        }

        if (model.FakultetId.HasValue && !_db.Fakulteti.Any(f => f.Id == model.FakultetId.Value))
        {
            ModelState.AddModelError(nameof(model.FakultetId), "Odabrani fakultet ne postoji.");
            ViewData["ToastMessage"] = "Unos studenta nije uspio. Odabrani fakultet ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateStudentCreateOptions(model);
            return View(model);
        }

        if (model.KolegijId.HasValue && !_db.Kolegiji.Any(k => k.Id == model.KolegijId.Value))
        {
            ModelState.AddModelError(nameof(model.KolegijId), "Odabrani kolegij ne postoji.");
            ViewData["ToastMessage"] = "Unos studenta nije uspio. Odabrani kolegij ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateStudentCreateOptions(model);
            return View(model);
        }

        var student = new Student(
            id: 0,
            ime: model.Ime.Trim(),
            prezime: model.Prezime.Trim(),
            datumUpisa: model.DatumUpisa)
        {
            FakultetId = model.FakultetId,
            KolegijId = model.KolegijId
        };

        _db.Studenti.Add(student);
        _db.SaveChanges();

        TempData["ToastMessage"] = "Student je uspjesno kreiran.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Search")]
    public IActionResult Search(string? q)
    {
        var studenti = BuildStudentQuery(q).ToList();
        return PartialView("_StudentCards", studenti);
    }

    [HttpGet("Autocomplete")]
    public IActionResult Autocomplete(string? q)
    {
        var query = _db.Studenti
            .AsNoTracking()
            .Include(s => s.Fakultet)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(s =>
                s.Ime.Contains(term) ||
                s.Prezime.Contains(term) ||
                ((s.Ime + " " + s.Prezime).Contains(term)));
        }

        var suggestions = query
            .OrderBy(s => s.Prezime)
            .ThenBy(s => s.Ime)
            .Take(8)
            .Select(s => new
            {
                value = s.Id,
                label = s.Ime + " " + s.Prezime,
                meta = s.Fakultet != null ? s.Fakultet.Naziv : string.Empty
            })
            .ToList();

        return Json(suggestions);
    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var student = _db.Studenti
            .AsNoTracking()
            .FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return NotFound();
        }

        var model = new StudentEditViewModel
        {
            Id = student.Id,
            Ime = student.Ime,
            Prezime = student.Prezime,
            DatumUpisa = student.DatumUpisa,
            FakultetId = student.FakultetId,
            KolegijId = student.KolegijId
        };
        PopulateStudentEditOptions(model);

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, StudentEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Azuriranje studenta nije uspjelo. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            PopulateStudentEditOptions(model);
            return View(model);
        }

        if (model.FakultetId.HasValue && !_db.Fakulteti.Any(f => f.Id == model.FakultetId.Value))
        {
            ModelState.AddModelError(nameof(model.FakultetId), "Odabrani fakultet ne postoji.");
            ViewData["ToastMessage"] = "Azuriranje studenta nije uspjelo. Odabrani fakultet ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateStudentEditOptions(model);
            return View(model);
        }

        if (model.KolegijId.HasValue && !_db.Kolegiji.Any(k => k.Id == model.KolegijId.Value))
        {
            ModelState.AddModelError(nameof(model.KolegijId), "Odabrani kolegij ne postoji.");
            ViewData["ToastMessage"] = "Azuriranje studenta nije uspjelo. Odabrani kolegij ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateStudentEditOptions(model);
            return View(model);
        }

        var student = _db.Studenti.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return NotFound();
        }

        student.Ime = model.Ime.Trim();
        student.Prezime = model.Prezime.Trim();
        student.DatumUpisa = model.DatumUpisa;
        student.FakultetId = model.FakultetId;
        student.KolegijId = model.KolegijId;

        _db.SaveChanges();

        TempData["ToastMessage"] = "Student je uspjesno azuriran.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Details), new { id = student.Id });
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

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var student = _db.Studenti.FirstOrDefault(s => s.Id == id);
        if (student is null)
        {
            return NotFound();
        }

        _db.Studenti.Remove(student);
        _db.SaveChanges();

        TempData["ToastMessage"] = "Student je uspjesno obrisan.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    private IQueryable<Student> BuildStudentQuery(string? q)
    {
        var query = _db.Studenti
            .AsNoTracking()
            .Include(s => s.DaneOcjene)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(s =>
                s.Ime.Contains(term) ||
                s.Prezime.Contains(term) ||
                ((s.Ime + " " + s.Prezime).Contains(term)));
        }

        return query
            .OrderBy(s => s.Prezime)
            .ThenBy(s => s.Ime);
    }

    private void PopulateStudentCreateOptions(StudentCreateViewModel model)
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

        model.Kolegiji = _db.Kolegiji
            .AsNoTracking()
            .OrderBy(k => k.Naziv)
            .Select(k => new LookupOption
            {
                Id = k.Id,
                Naziv = k.Naziv
            })
            .ToList();
    }

    private void PopulateStudentEditOptions(StudentEditViewModel model)
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

        model.Kolegiji = _db.Kolegiji
            .AsNoTracking()
            .OrderBy(k => k.Naziv)
            .Select(k => new LookupOption
            {
                Id = k.Id,
                Naziv = k.Naziv
            })
            .ToList();
    }
}