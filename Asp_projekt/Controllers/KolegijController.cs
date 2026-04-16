using Asp_projekt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Asp_projekt.Controllers;

public class KolegijController : Controller
{
    private readonly KolegijMockRepository _kolegijRepository;

    public KolegijController(KolegijMockRepository kolegijRepository)
    {
        _kolegijRepository = kolegijRepository;
    }

    public IActionResult Index()
    {
        return View(_kolegijRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var kolegij = _kolegijRepository.GetById(id);
        if (kolegij is null)
        {
            return NotFound();
        }

        return View(kolegij);
    }
}