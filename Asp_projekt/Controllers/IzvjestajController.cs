using Asp_projekt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Asp_projekt.Controllers;

public class IzvjestajController : Controller
{
    private readonly IzvjestajMockRepository _izvjestajRepository;

    public IzvjestajController(IzvjestajMockRepository izvjestajRepository)
    {
        _izvjestajRepository = izvjestajRepository;
    }

    public IActionResult Index()
    {
        return View(_izvjestajRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var izvjestaj = _izvjestajRepository.GetById(id);
        if (izvjestaj is null)
        {
            return NotFound();
        }

        return View(izvjestaj);
    }
}
