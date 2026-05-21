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
        var ocjene = BuildOcjenaQuery(null).ToList();

        return View(ocjene);
    }

    [HttpGet("Search")]
    public IActionResult Search(string? q)
    {
        var ocjene = BuildOcjenaQuery(q).ToList();
        return PartialView("_OcjenaCards", ocjene);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        var model = new OcjenaCreateViewModel();
        PopulateCreateOptions(model);

        return View(model);
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(OcjenaCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Unos ocjene nije uspio. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            PopulateCreateOptions(model);

            return View(model);
        }

        var profesor = _db.Profesori.FirstOrDefault(p => p.Id == model.ProfesorId);
        if (profesor is null)
        {
            ModelState.AddModelError(nameof(model.ProfesorId), "Odabrani profesor ne postoji.");
            ViewData["ToastMessage"] = "Unos ocjene nije uspio. Odabrani profesor ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateCreateOptions(model);
            return View(model);
        }

        var student = _db.Studenti.FirstOrDefault(s => s.Id == model.StudentId);
        if (student is null)
        {
            ModelState.AddModelError(nameof(model.StudentId), "Odabrani student ne postoji.");
            ViewData["ToastMessage"] = "Unos ocjene nije uspio. Odabrani student ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateCreateOptions(model);
            return View(model);
        }

        var kolegij = _db.Kolegiji.FirstOrDefault(k => k.Id == model.KolegijId);
        if (kolegij is null)
        {
            ModelState.AddModelError(nameof(model.KolegijId), "Odabrani kolegij ne postoji.");
            ViewData["ToastMessage"] = "Unos ocjene nije uspio. Odabrani kolegij ne postoji.";
            ViewData["ToastType"] = "warning";
            PopulateCreateOptions(model);
            return View(model);
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

        TempData["ToastMessage"] = "Ocjena je uspjesno kreirana.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var ocjena = _db.Ocjene
            .AsNoTracking()
            .FirstOrDefault(o => o.Id == id);
        if (ocjena is null)
        {
            return NotFound();
        }

        var model = new OcjenaEditViewModel
        {
            Id = ocjena.Id,
            ProfesorId = ocjena.ProfesorId,
            StudentId = ocjena.StudentId,
            KolegijId = ocjena.KolegijId,
            Vrijednost = ocjena.Vrijednost,
            Tip = ocjena.Tip,
            Komentar = ocjena.Komentar,
            DatumOcjene = ocjena.DatumOcjene
        };
        PopulateEditOptions(model);

        return View(model);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, OcjenaEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["ToastMessage"] = "Azuriranje ocjene nije uspjelo. Provjerite podatke.";
            ViewData["ToastType"] = "error";
            PopulateEditOptions(model);
            return View(model);
        }

        var ocjena = _db.Ocjene.FirstOrDefault(o => o.Id == id);
        if (ocjena is null)
        {
            return NotFound();
        }

        var profesorExists = _db.Profesori.Any(p => p.Id == model.ProfesorId);
        var studentExists = _db.Studenti.Any(s => s.Id == model.StudentId);
        var kolegijExists = _db.Kolegiji.Any(k => k.Id == model.KolegijId);
        if (!profesorExists || !studentExists || !kolegijExists)
        {
            ModelState.AddModelError(string.Empty, "Odabrani profesor, student ili kolegij vise ne postoji.");
            ViewData["ToastMessage"] = "Azuriranje ocjene nije uspjelo. Povezani podaci ne postoje.";
            ViewData["ToastType"] = "warning";
            PopulateEditOptions(model);
            return View(model);
        }

        ocjena.ProfesorId = model.ProfesorId;
        ocjena.StudentId = model.StudentId;
        ocjena.KolegijId = model.KolegijId;
        ocjena.Vrijednost = model.Vrijednost;
        ocjena.Tip = model.Tip;
        ocjena.Komentar = model.Komentar;
        ocjena.DatumOcjene = model.DatumOcjene;

        _db.SaveChanges();

        TempData["ToastMessage"] = "Ocjena je uspjesno azurirana.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Details), new { id = ocjena.Id });
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

        TempData["ToastMessage"] = "Ocjena je uspjesno obrisana.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    private void PopulateCreateOptions(OcjenaCreateViewModel model)
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
    }

    private void PopulateEditOptions(OcjenaEditViewModel model)
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
    }

    private IQueryable<Ocjena> BuildOcjenaQuery(string? q)
    {
        var query = _db.Ocjene
            .AsNoTracking()
            .Include(o => o.Student)
            .Include(o => o.Profesor)
            .Include(o => o.Kolegij)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(o =>
                o.Student.Ime.Contains(term) ||
                o.Student.Prezime.Contains(term) ||
                o.Profesor.Ime.Contains(term) ||
                o.Profesor.Prezime.Contains(term) ||
                o.Kolegij.Naziv.Contains(term) ||
                o.Komentar.Contains(term));
        }

        return query
            .OrderByDescending(o => o.DatumOcjene)
            .ThenBy(o => o.Student.Prezime)
            .ThenBy(o => o.Student.Ime);
    }
}
