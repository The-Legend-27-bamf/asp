using Asp_projekt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Asp_projekt.Controllers;

public class FakultetController : Controller
{
    private readonly FakultetMockRepository _fakultetRepository;

    public FakultetController(FakultetMockRepository fakultetRepository)
    {
        _fakultetRepository = fakultetRepository;
    }

    public IActionResult Index()
    {
        return View(_fakultetRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var fakultet = _fakultetRepository.GetById(id);
        if (fakultet is null)
        {
            return NotFound();
        }

        return View(fakultet);
    }
}
