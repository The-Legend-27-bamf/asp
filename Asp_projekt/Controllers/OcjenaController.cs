using Asp_projekt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Asp_projekt.Controllers;

public class OcjenaController : Controller
{
    private readonly OcjenaMockRepository _ocjenaRepository;

    public OcjenaController(OcjenaMockRepository ocjenaRepository)
    {
        _ocjenaRepository = ocjenaRepository;
    }

    public IActionResult Index()
    {
        return View(_ocjenaRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var ocjena = _ocjenaRepository.GetById(id);
        if (ocjena is null)
        {
            return NotFound();
        }

        return View(ocjena);
    }
}
