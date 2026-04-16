using Asp_projekt.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Asp_projekt.Controllers;

public class ProfesorController : Controller
{
    private readonly ProfesorMockRepository _profesorRepository;

    public ProfesorController(ProfesorMockRepository profesorRepository)
    {
        _profesorRepository = profesorRepository;
    }

    public IActionResult Index()
    {
        return View(_profesorRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        var profesor = _profesorRepository.GetById(id);
        if (profesor is null)
        {
            return NotFound();
        }

        return View(profesor);
    }
}